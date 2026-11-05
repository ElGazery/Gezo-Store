using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Reviews.Dtos;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Reviews;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Reviews.Queries.GetProductReviewsQuery
{
    public record GetProductReviewsQuery(int ProductId) : IRequest<ApiResponse<IReadOnlyList<ReviewDto>>>
    {
    }

    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, ApiResponse<IReadOnlyList<ReviewDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IReadOnlyList<ReviewDto>>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.GenericRepository<Product>().GetbyIdAsync(request.ProductId);
            if (product is null)
                throw new NotFoundCustomException("Product not found");

            var spec = new GetProductReviewsSpecification(request.ProductId);
            var reviews = await _unitOfWork.GenericRepository<Review>().GetAllWithSpecAsync(spec, cancellationToken);
            var dtos = _mapper.Map<IReadOnlyList<ReviewDto>>(reviews);

            return new ApiResponse<IReadOnlyList<ReviewDto>>(dtos, "Reviews retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

