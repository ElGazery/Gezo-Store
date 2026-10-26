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

namespace Gezo.Application.Features.Accounts.Commands.ForgetPasswordCommand
{
    public record ForgetPasswordCommand(string email) :IRequest<ApiResponse<bool>>
    {
    }

    public class ForgetPasswrodCommandHandler : IRequestHandler<ForgetPasswordCommand, ApiResponse<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordResetService _passwordResetService;
        public ForgetPasswrodCommandHandler(UserManager<ApplicationUser> userManager, IPasswordResetService passwordResetService)
        {
            _userManager = userManager;
            _passwordResetService = passwordResetService;
            
        }
        public async Task<ApiResponse<bool>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.email);
            if (user is not null)
                await _passwordResetService.SendResetPasswordEmailAsync(user, cancellationToken);

            return new ApiResponse<bool>(
                    true,
                     "If an account with that email exists, a reset link will be sent.",
                      StatusCodes.Status200OK
                    );      

        }
    }
}
