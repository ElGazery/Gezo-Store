using AutoMapper;
using Gezo.Application.Features.Basket.Dtos;
using Gezo.Domain.Entities;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Common.Mappings
{
    public class BasketProfile: Profile
    {
        public BasketProfile()
        {
            CreateMap<Product, BasketItemDto>()
           .ForMember(dest => dest.ProductId,
               opt => opt.MapFrom(src => src.Id))

           .ForMember(dest => dest.ProductName,
               opt => opt.MapFrom(src => src.Name))

           .ForMember(dest => dest.Price,
               opt => opt.MapFrom(src => src.DiscountPrice ?? src.Price))

           .ForMember(dest => dest.PictureUrl,
               opt => opt.MapFrom(src =>
                   src.Images
                       .Where(i => i.IsPrimary)
                       .Select(i => i.ImageUrl)
                       .FirstOrDefault()
                   ??
                   src.Images
                       .OrderBy(i => i.DisplayOrder)
                       .Select(i => i.ImageUrl)
                       .FirstOrDefault()
               ))

           .ForMember(dest => dest.Quantity,
               opt => opt.Ignore());

            CreateMap<BasketItemDto, OrderItem>()
                .ForMember(dist => dist.UnitPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dist => dist.PictureUrl, opt => opt.MapFrom(src => src.PictureUrl ?? string.Empty));

        }
    }
}
