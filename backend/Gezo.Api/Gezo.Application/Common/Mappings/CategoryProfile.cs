using AutoMapper;
using Gezo.Application.Features.Categories.Commands.UpdateCategoryCommand;
using Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery;
using Gezo.Application.Features.Categories.Queries.GetCategoryByIdQuery;
using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Common.Mappings
{
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetAllCategoriesQueryDto>();
            CreateMap<Category, GetCategoryByIdQueryDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<UpdateCategoryCommand, Category>()
             .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
               .ForMember(dest => dest.Id, opt => opt.Ignore());


        }

    }
}
