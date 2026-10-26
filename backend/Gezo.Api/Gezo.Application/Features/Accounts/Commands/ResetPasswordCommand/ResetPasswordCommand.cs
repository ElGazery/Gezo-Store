using Gezo.ApiResponseBase.Response;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.ResetPasswordCommand
{
    public record ResetPasswordCommand(string userId,string token,string newPassword) :IRequest<ApiResponse<bool>>
    {
    }
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ApiResponse<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApiResponse<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);
            if (user is null)
                throw new BadRequestCustomException("User Not found");
            var result = await _userManager.ResetPasswordAsync(user, request.token, request.newPassword);
            if (!result.Succeeded)
                throw new BadRequestCustomException("Reset Password faild please try again");

            return new ApiResponse<bool>(true, "Password reset successfuly", StatusCodes.Status200OK);



        }
    }

}
