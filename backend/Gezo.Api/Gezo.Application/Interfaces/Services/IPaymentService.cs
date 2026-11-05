using Gezo.Application.Features.Payments.dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<PaymentIntentResultDto> CreatePaymentIntentAsync(decimal amount, string currency, string orderId, CancellationToken cancellationToken);
        Task<PaymentIntentResultDto> GetPaymentIntentAsync(string paymentIntentId, CancellationToken cancellationToken = default);
        Task<bool> ProcessWebhookAsync(string json, string? stripeSignature, CancellationToken cancellationToken = default);
    }
}
