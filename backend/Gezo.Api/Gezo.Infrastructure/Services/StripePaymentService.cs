using Gezo.Application.Common.Settings;
using Gezo.Application.Features.Payments.dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.Orders;
using Gezo.Domain.Entities;
using Gezo.Domain.Enums;
using Gezo.Domain.Exceptions;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Infrastructure.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly PaymentIntentService? _paymentIntentService;
        private readonly IUnitOfWork _unitOfWork;

        public StripePaymentService(IOptions<StripeSettings> stripeSettings, IUnitOfWork unitOfWork)
        {
            _stripeSettings = stripeSettings.Value;
            if (!string.IsNullOrWhiteSpace(_stripeSettings.SecretKey))
            {
                var stripeClient = new StripeClient(_stripeSettings.SecretKey);
                _paymentIntentService = new PaymentIntentService(stripeClient);
            }
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentIntentResultDto> CreatePaymentIntentAsync(decimal amount, string currency, string orderId, CancellationToken cancellationToken)
        {
            if (_paymentIntentService == null)
                throw new BadRequestCustomException("Stripe SecretKey is not configured.");

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            var amountInSmallestUnit = checked(amount * 100m);

            if (amountInSmallestUnit != decimal.Truncate(amountInSmallestUnit))
                throw new ArgumentException("Amount cannot have more than two decimal places.");

            var stripeAmount = decimal.ToInt64(amountInSmallestUnit);

            var idempotencyKey = $"order-{orderId}-payment-intent";

            var requestOptions = new RequestOptions
            {
                IdempotencyKey = idempotencyKey
            };

            var options = new PaymentIntentCreateOptions
            {
                Amount = stripeAmount,
                Currency = currency.ToLowerInvariant(),
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                },
                Metadata = new Dictionary<string, string> { { "OrderId", orderId } }
            };

            var paymentIntent = await _paymentIntentService.CreateAsync(options, requestOptions, cancellationToken: cancellationToken);
            return new PaymentIntentResultDto(paymentIntent.Id, paymentIntent.ClientSecret, paymentIntent.Status);
        }

        public async Task<PaymentIntentResultDto> GetPaymentIntentAsync(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            if (_paymentIntentService == null)
                throw new BadRequestCustomException("Stripe SecretKey is not configured.");

            if (string.IsNullOrWhiteSpace(paymentIntentId))
                throw new ArgumentException("PaymentIntentId is required.");

            var paymentIntent = await _paymentIntentService.GetAsync(
                paymentIntentId,
                cancellationToken: cancellationToken
            );

            return new PaymentIntentResultDto(
                paymentIntent.Id,
                paymentIntent.ClientSecret,
                paymentIntent.Status
            );
        }

        public async Task<bool> ProcessWebhookAsync(string json, string? stripeSignature, CancellationToken cancellationToken = default)
        {
            Event stripeEvent;

            try
            {
                if (!string.IsNullOrEmpty(_stripeSettings.WebhookSecret) && !string.IsNullOrEmpty(stripeSignature))
                {
                    stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _stripeSettings.WebhookSecret);
                }
                else
                {
                    stripeEvent = EventUtility.ParseEvent(json);
                }
            }
            catch (Exception ex)
            {
                throw new BadRequestCustomException($"Webhook Error: {ex.Message}");
            }

            if (stripeEvent.Data.Object is PaymentIntent paymentIntent)
            {
                Order? order = null;

                if (paymentIntent.Metadata != null && paymentIntent.Metadata.TryGetValue("OrderId", out var orderIdStr) && int.TryParse(orderIdStr, out int orderId))
                {
                    order = await _unitOfWork.GenericRepository<Order>().GetbyIdAsync(orderId);
                }

                if (order is null)
                {
                    var spec = new OrderByPaymentIntentIdSpecification(paymentIntent.Id);
                    order = await _unitOfWork.GenericRepository<Order>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
                }

                if (order is not null)
                {
                    if (stripeEvent.Type == "payment_intent.succeeded")
                    {
                        order.PaymentStatus = PaymentStatus.Paid;
                        order.Status = OrderStatus.PaymentReceived;
                    }
                    else if (stripeEvent.Type == "payment_intent.payment_failed")
                    {
                        order.PaymentStatus = PaymentStatus.Failed;
                        order.Status = OrderStatus.PaymentFailed;
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return true;
        }
    }
}
