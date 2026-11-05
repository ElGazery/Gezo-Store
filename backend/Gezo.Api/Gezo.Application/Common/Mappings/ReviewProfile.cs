using AutoMapper;
using Gezo.Application.Features.Reviews.Dtos;
using Gezo.Domain.Entities;

namespace Gezo.Application.Common.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName ?? src.User.UserName));
        }
    }
}

