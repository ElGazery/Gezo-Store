using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Specifications.Categories;
using Gezo.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery
{
    public record class GetAllCategoriesQuery:IRequest<ApiResponse<IReadOnlyList<GetAllCategoriesQueryDto>>>
    {
    }

    internal class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoriesQuery,ApiResponse<IReadOnlyList<GetAllCategoriesQueryDto>>> 
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public GetAllCategoryQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IReadOnlyList<GetAllCategoriesQueryDto>>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
        {
            var spec = new GetAllCategoriesSpacification();
            var categoreies =await _unitOfWork.GenericRepository<Category>().GetAllWithSpecAsync(spec, cancellationToken);
            var categoreiesDto = _mapper.Map<IReadOnlyList<GetAllCategoriesQueryDto>>(categoreies);

            return  new ApiResponse<IReadOnlyList<GetAllCategoriesQueryDto>>(categoreiesDto,"success",200);
            
                

        }
    }

}
