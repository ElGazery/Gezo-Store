using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Brands;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Brands.Queries.GetBrandByIdQuery
{
    public record GetBrandByIdQuery(int id) : IRequest<ApiResponse<GetBrandByIdQueryDto>>
    {
    }

    internal class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, ApiResponse<GetBrandByIdQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetBrandByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GetBrandByIdQueryDto>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetBrandByIdSpecification(request.id);
            var brand = await _unitOfWork.GenericRepository<Brand>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
            if (brand is null)
                throw new NotFoundCustomException("Brand not found");

            var dto = _mapper.Map<GetBrandByIdQueryDto>(brand);

            return new ApiResponse<GetBrandByIdQueryDto>(dto, "success", StatusCodes.Status200OK);
        }
    }
}

