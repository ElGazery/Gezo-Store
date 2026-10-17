using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Brands;
using Gezo.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery
{
    public record GetAllBrandsQuery :IRequest<ApiResponse<IReadOnlyList<GetAllBrandsQueryDto>>>
    {
    }

    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, ApiResponse<IReadOnlyList<GetAllBrandsQueryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllBrandsQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IReadOnlyList<GetAllBrandsQueryDto>>> Handle(GetAllBrandsQuery query, CancellationToken cancellationToken)
        {
            var spec = new GetAllBrandsSpecification();
            var brands = await _unitOfWork.GenericRepository<Brand>().GetAllWithSpecAsync(spec, cancellationToken);
            var BrandsDto = _mapper.Map<IReadOnlyList<GetAllBrandsQueryDto>>(brands);

            return new ApiResponse<IReadOnlyList<GetAllBrandsQueryDto>>(BrandsDto, "success", 200);
        }
    }

}
