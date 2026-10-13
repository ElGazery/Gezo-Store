using AutoMapper;
using Gezo.Application.Features.Products.Queries.GetProdcutByIdQuery;
using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Gezo.Application.Common.Mappings
{
    public class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductImage, string>().ConvertUsing(src=>src.ImageUrl);

            CreateMap<Product,GetProductByIdQueryDto>().
                ForMember(dest=>dest.CategoryName,opt=>opt.MapFrom(src=>src.Category.Name))
                .ForMember(dest=>dest.BrandName,opt=>opt.MapFrom(src=>src.Brand.Name)); 
            
                

        }
    }
}
