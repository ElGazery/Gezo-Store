using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Payments.dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Payments.Queries.GetPaymentStatusQuery
{
    public record GetPaymentStatusQuery(int OrderId) : IRequest<ApiResponse<PaymentStatusDto>>
    {
    }

    internal class GetPaymentStatusQueryHandler : IRequestHandler<GetPaymentStatusQuery, ApiResponse<PaymentStatusDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public GetPaymentStatusQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<PaymentStatusDto>> Handle(GetPaymentStatusQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.GenericRepository<Order>().GetbyIdAsync(request.OrderId);
            if (order is null)
                throw new NotFoundCustomException("Order not found");

            var user = await _currentUserService.GetCurrentUser();
            if (user is null)
                throw new BadRequestCustomException("User not found");

            if (order.UserId != user.Id)
                throw new BadRequestCustomException("There is something wrong");

            var dto = new PaymentStatusDto
            {
                OrderId = order.Id,
                PaymentStatus = order.PaymentStatus.ToString(),
                OrderStatus = order.Status.ToString(),
                PaymentMethod = order.PaymentMethod.ToString()
            };

            return new ApiResponse<PaymentStatusDto>(dto, "Payment status retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

