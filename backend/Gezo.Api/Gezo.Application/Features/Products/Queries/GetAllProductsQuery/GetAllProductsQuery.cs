using AutoMapper;
using Gezo.Api.ResponseBase.Paginations;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Products;
using Gezo.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
namespace Gezo.Application.Features.Products.Queries.GetAllProductsQuery
{
    public  record GetAllProductsQuery
        (int PageIndex=1,
        int PageSize=10,
        int?BrandId=null,
        int?CategoryId=null,
        List<string>?Sort=null,
        decimal? MinPrice=null,
        decimal? MaxPrice=null,
        string?Search = null)
    : IRequest<ApiResponse<PaginationResponse<GetAllProductsQueryDto>>>
    {
       
    }


    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, ApiResponse<PaginationResponse<GetAllProductsQueryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllProductsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
      public async Task<ApiResponse<PaginationResponse<GetAllProductsQueryDto>>> Handle(GetAllProductsQuery query, CancellationToken cancellation)
        {
            var spec = new ProductsWithFiltersSpecification(query);

            var repo = _unitOfWork.GenericRepository<Product>();

            var totalCount = await repo.CountWithSpecAsync(spec);

            var products = await repo.GetAllWithSpecAsync(spec);

            var mapProducts = _mapper.Map<IReadOnlyList<GetAllProductsQueryDto>>(products);

            var result = new PaginationResponse<GetAllProductsQueryDto>( query.PageIndex,query.PageSize,totalCount,mapProducts);

            return new  ApiResponse<PaginationResponse<GetAllProductsQueryDto>>(result, "success",200);
        }
    }
}
