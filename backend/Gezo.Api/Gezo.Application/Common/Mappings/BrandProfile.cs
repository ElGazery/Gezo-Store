using AutoMapper;
using Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery;
using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Common.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, GetAllBrandsQueryDto>();
        }
    }
}
