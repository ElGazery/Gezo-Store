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

namespace Gezo.Application.Features.Accounts.Commands.ChangePasswordCommand
{
    public record ChangePasswordCommand(string oldPassword,string newPassword,string confirmPassword):IRequest<ApiResponse<bool>>
    {
    }
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ApiResponse<bool>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetCurrentUser();
            if (user is null)
                throw new BadRequestCustomException("User not found");
            
           var isChange= await _userManager.ChangePasswordAsync(user, request.oldPassword, request.newPassword);
            if (!isChange.Succeeded)
            {
                var errors = string.Join( ", ", isChange.Errors.Select(e => e.Description));
                throw new BadRequestCustomException(errors);

            }

            return new ApiResponse<bool>(true, "password change successfuly",StatusCodes.Status200OK);  
        }
    }
}
