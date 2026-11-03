using AutoMapper;
using Gezo.Application.Features.Addresses.Commands.AddAddressCommand;
using Gezo.Application.Features.Addresses.Commands.UpdateAddressCommand;
using Gezo.Application.Features.Addresses.Dtos;
using Gezo.Domain.Entities;

namespace Gezo.Application.Common.Mappings
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<ApplicationUser, AddressDto>();
            CreateMap<AddressDto, ApplicationUser>();
            CreateMap<AddAddressCommand, ApplicationUser>();
            CreateMap<UpdateAddressCommand, ApplicationUser>();
        }
    }
}

