using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Enums;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Payments.Commands.CreatePaymentIntentCommand
{
    public record CreatePaymentIntentCommand(int orderId):IRequest<ApiResponse<CreatePaymentIntentDto>>
    {
    }

    internal class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, ApiResponse<CreatePaymentIntentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentService _paymentService;

        public CreatePaymentIntentCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<ApiResponse<CreatePaymentIntentDto>> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.GenericRepository<Order>().GetbyIdAsync(request.orderId);
            if (order is null)
                throw new BadRequestCustomException("Order not found");

            var user = await _currentUserService.GetCurrentUser();
            if (user is null)
                throw new BadRequestCustomException("User not found");

            if (user.Id != order.UserId)
                throw new BadRequestCustomException("There is something wrong ");

            if (order.PaymentMethod != PaymentMethod.Online)
                throw new BadRequestCustomException("PaymentMethod is not correct");

            if (order.PaymentStatus != PaymentStatus.Pending)
                throw new BadRequestCustomException("Can't complet the order payment");

            if (!string.IsNullOrWhiteSpace(order.PaymentIntentId))
            {
                var existingPaymentIntent = await _paymentService.GetPaymentIntentAsync(
                    order.PaymentIntentId,
                    cancellationToken
                );

                var dto = new CreatePaymentIntentDto
                {
                    PaymentIntentId = existingPaymentIntent.PaymentIntentId,
                    ClientSecret = existingPaymentIntent.ClientSecret,
                    Status = order.PaymentStatus.ToString()
                };

                return new ApiResponse<CreatePaymentIntentDto>(dto, "Payment intent retrieved successfully", StatusCodes.Status200OK);
            }

            var paymentIntent = await _paymentService.CreatePaymentIntentAsync(
                order.Total,
                "usd",
                order.Id.ToString(),
                cancellationToken
            );

            order.PaymentIntentId = paymentIntent.PaymentIntentId;
            await _unitOfWork.SaveChangesAsync();

            var resultDto = new CreatePaymentIntentDto
            {
                PaymentIntentId = paymentIntent.PaymentIntentId,
                ClientSecret = paymentIntent.ClientSecret,
                Status = order.PaymentStatus.ToString()
            };

            return new ApiResponse<CreatePaymentIntentDto>(resultDto, "Payment intent created successfully", StatusCodes.Status200OK);
        }
    }
}
