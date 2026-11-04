using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Domain.Entities;
using Gezo.Domain.Enums;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Orders.Commands.UpdateOrderStatusCommand
{
    public record UpdateOrderStatusCommand : IRequest<ApiResponse<bool>>
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, ApiResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.GenericRepository<Order>().GetbyIdAsync(request.Id);
            if (order is null)
                throw new NotFoundCustomException("Order not found");

            order.Status = request.Status;

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>(true, "Order status updated successfully", StatusCodes.Status200OK);
        }
    }
}

