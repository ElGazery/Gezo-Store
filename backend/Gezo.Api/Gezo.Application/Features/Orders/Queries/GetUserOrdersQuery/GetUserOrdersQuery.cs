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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Orders.Queries.GetUserOrdersQuery
{
    public record GetUserOrdersQuery : IRequest<ApiResponse<IReadOnlyList<OrderToReturnDto>>>
    {
    }

    public class GetUserOrdersQueryHandler : IRequestHandler<GetUserOrdersQuery, ApiResponse<IReadOnlyList<OrderToReturnDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetUserOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<IReadOnlyList<OrderToReturnDto>>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new BadRequestCustomException("User not found");

            var spec = new OrdersForUserSpecification(userId);
            var orders = await _unitOfWork.GenericRepository<Order>().GetAllWithSpecAsync(spec, cancellationToken);
            var dtos = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

            return new ApiResponse<IReadOnlyList<OrderToReturnDto>>(dtos, "Orders retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

