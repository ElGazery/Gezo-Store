using AutoMapper;
using Gezo.Application.Features.Wishlist.Dtos;
using Gezo.Domain.Entities;
using System.Linq;

namespace Gezo.Application.Common.Mappings
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            CreateMap<WishlistItem, WishlistItemDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))

                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.DiscountPrice, opt => opt.MapFrom(src => src.Product.DiscountPrice))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Product.Brand.Name))
                .ForMember(dest => dest.InStock, opt => opt.MapFrom(src => src.Product.StockQuantity > 0))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src =>
                    src.Product.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()?? src.Product.Images
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault()));
        }
    }
}

