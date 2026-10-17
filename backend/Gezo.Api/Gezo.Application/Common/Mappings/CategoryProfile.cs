using AutoMapper;
using Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery;
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
        }

    }
}
