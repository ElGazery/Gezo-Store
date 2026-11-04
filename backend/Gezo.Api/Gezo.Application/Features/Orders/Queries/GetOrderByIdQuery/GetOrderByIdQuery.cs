using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Orders.Dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.Orders;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Orders.Queries.GetOrderByIdQuery
{
    public record GetOrderByIdQuery(int Id) : IRequest<ApiResponse<OrderToReturnDto>>
    {
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderToReturnDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<OrderToReturnDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");

            var spec = new OrderWithItemsByIdAndUserSpecification(request.Id, userId);
            var order = await _unitOfWork.GenericRepository<Order>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
            if (order is null)
                throw new NotFoundCustomException("Order not found");

            var dto = _mapper.Map<OrderToReturnDto>(order);
            return new ApiResponse<OrderToReturnDto>(dto, "Order retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

