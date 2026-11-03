using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Addresses.Dtos;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Addresses.Commands.UpdateAddressCommand
{
    public record UpdateAddressCommand : IRequest<ApiResponse<AddressDto>>
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Governorate { get; set; } = string.Empty;
        public int? PostalCode { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, ApiResponse<AddressDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UpdateAddressCommandHandler(
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AddressDto>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetCurrentUser();
            if (user is null)
                throw new BadRequestCustomException("User not found");

            _mapper.Map(request, user);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestCustomException(errors);
            }

            var dto = _mapper.Map<AddressDto>(user);
            return new ApiResponse<AddressDto>(dto, "Address updated successfully", StatusCodes.Status200OK);
        }
    }
}

