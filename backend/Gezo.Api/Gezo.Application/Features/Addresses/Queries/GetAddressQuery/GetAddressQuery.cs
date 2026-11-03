using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Features.Addresses.Dtos;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gezo.Application.Features.Addresses.Queries.GetAddressQuery
{
    public record GetAddressQuery : IRequest<ApiResponse<AddressDto>>
    {
    }

    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, ApiResponse<AddressDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetAddressQueryHandler(ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<AddressDto>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetCurrentUser();
            if (user is null)
                throw new BadRequestCustomException("User not found");

            if (string.IsNullOrWhiteSpace(user.Street) &&
                string.IsNullOrWhiteSpace(user.City) &&
                string.IsNullOrWhiteSpace(user.Governorate) &&
                user.PostalCode == null)
            {
                throw new NotFoundCustomException("Address not found");
            }

            var dto = _mapper.Map<AddressDto>(user);
            return new ApiResponse<AddressDto>(dto, "Address retrieved successfully", StatusCodes.Status200OK);
        }
    }
}

