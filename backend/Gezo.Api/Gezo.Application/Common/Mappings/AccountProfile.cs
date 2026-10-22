using AutoMapper;
using Gezo.Application.Features.Accounts.Commands.LoginCommand;
using Gezo.Application.Features.Accounts.Commands.RegisterCommand;
using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Common.Mappings
{
    public class AccountProfile :Profile
    {
        public AccountProfile()
        {

     
            CreateMap<RegisterCommand, ApplicationUser>()
                .ForMember(dist => dist.UserName, option => option.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, RegisterResponseDto>()
                .ForMember(dist => dist.UserId, option => option.MapFrom(src => src.Id))
                .ForMember(dist => dist.Token, option => option.Ignore());

            CreateMap<ApplicationUser, LoginResponseDto>().
                 ForMember(dist => dist.UserId, option => option.MapFrom(src => src.Id))
                .ForMember(dist => dist.Token, option => option.Ignore());
        }
    }
}
