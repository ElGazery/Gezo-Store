using Gezo.ApiResponseBase.Response;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.ConfirmEmailCommand
{
    public record ConfirmEmailCommand(string userId,string  token ): IRequest<ApiResponse<bool>>
    {
    }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResponse<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManger;
        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManger)
        {
            _userManger = userManger;
        }
        public async Task<ApiResponse<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManger.FindByIdAsync(request.userId);
            if (user is null)
                throw new NotFoundCustomException("User not found");
            var result = await _userManger.ConfirmEmailAsync(user, request.token);
            if (!result.Succeeded)
            {
                   throw new BadRequestCustomException( "Email confirmation failed");
            }
            return new ApiResponse<bool>(true, "Email Confirm succfully ",StatusCodes.Status200OK);

        }
    }
}
