
using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
namespace Gezo.Application.Features.Products.Queries.GetProdcutByIdQuery
{
    public record GetProdcutByIdQuery(int id) :IRequest<ApiResponse<GetProductByIdQueryDto>>
    {
    }

   internal class GetProductByIdHandler:IRequestHandler<GetProdcutByIdQuery, ApiResponse<GetProductByIdQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProductByIdHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GetProductByIdQueryDto>> Handle(GetProdcutByIdQuery query, CancellationToken cancellationToken)
        {
            var getProduct = await _unitOfWork.GenericRepository<Product>().GetbyIdAsync(query.id);
            if(getProduct == null)
            {
                return  new ApiResponse<GetProductByIdQueryDto>("Product not found",404);
            }
            var product = _mapper.Map<GetProductByIdQueryDto>(getProduct);

            return new ApiResponse<GetProductByIdQueryDto>(product, "success", 200);
            

        }
    }

}
