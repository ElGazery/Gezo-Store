using AutoMapper;
using Gezo.Api.ResponseBase.Paginations;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Orders.Dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Orders;
using Gezo.Domain.Entities;
using Gezo.Domain.Enums;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Orders.Queries.GetAllOrdersForAdminQuery
{
    public record GetAllOrdersForAdminQuery(
        int PageIndex = 1,
        int PageSize = 10,
        OrderStatus? Status = null,
        PaymentStatus? PaymentStatus = null,
        string? Search = null)
    : IRequest<ApiResponse<PaginationResponse<OrderToReturnDto>>>
    {
    }

    public class GetAllOrdersForAdminHandler : IRequestHandler<GetAllOrdersForAdminQuery, ApiResponse<PaginationResponse<OrderToReturnDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrdersForAdminHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginationResponse<OrderToReturnDto>>> Handle(GetAllOrdersForAdminQuery query, CancellationToken cancellationToken)
        {
            var spec = new OrdersWithFiltersForAdminSpecification(query);
            var repo = _unitOfWork.GenericRepository<Order>();

            var totalCount = await repo.CountWithSpecAsync(spec, cancellationToken);

            var orders = await repo.GetAllWithSpecAsync(spec, cancellationToken);
            var mapOrders = _mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);

            var result = new PaginationResponse<OrderToReturnDto>(query.PageIndex, query.PageSize, totalCount, mapOrders);

            return new ApiResponse<PaginationResponse<OrderToReturnDto>>(result, "Orders retrieved successfully", 200);
        }
    }
}

