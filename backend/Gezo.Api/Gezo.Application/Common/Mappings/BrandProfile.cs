using AutoMapper;
using Gezo.Application.Features.Brands.Commands.AddBrandCommand;
using Gezo.Application.Features.Brands.Commands.UpdateBrandCommand;
using Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery;
using Gezo.Application.Features.Brands.Queries.GetBrandByIdQuery;
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
            CreateMap<Brand, GetBrandByIdQueryDto>();
            CreateMap<AddBrandCommand, Brand>()
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore());
            CreateMap<UpdateBrandCommand, Brand>()
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
