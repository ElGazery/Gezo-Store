using AutoMapper;
using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Queries.GetCurrentUserQuery
{
    public record GetCurrentUserQuery:IRequest<ApiResponse<GetCurrentUserDto>>
    {
    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, ApiResponse<GetCurrentUserDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(ICurrentUserService currentUserService, UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<ApiResponse<GetCurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
                           
            var user = _currentUserService.GetCurrentUser();
            var dto = _mapper.Map<GetCurrentUserDto>(user);
            return new ApiResponse<GetCurrentUserDto>(
                dto,
                "Success",
                StatusCodes.Status200OK);
        }
    }

}
