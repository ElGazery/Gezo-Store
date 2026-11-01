using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Categories;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Queries.GetCategoryByIdQuery
{
    public record GetCategoryByIdQuery(int id):IRequest<ApiResponse<GetCategoryByIdQueryDto>>
    {
    }
    internal class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse<GetCategoryByIdQueryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<GetCategoryByIdQueryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new GetCategoryByIdSpecifiction(request.id);
            var category = await _unitOfWork.GenericRepository<Category>().GetFirstOrDefaultWithSpecAsync(spec,cancellationToken);
            if (category is null)
                throw new NotFoundCustomException("Category not found");
            var dto = _mapper.Map<GetCategoryByIdQueryDto>(category);

            return new ApiResponse<GetCategoryByIdQueryDto>(dto, "success", StatusCodes.Status200OK);
        }
    }
}
