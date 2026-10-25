using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.LogoutCommand
{
    public record LogoutCommand:IRequest<ApiResponse<string>>
    {
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResponse<string>>
    {
        private readonly IRefreshTokenCookieService _refreshTokenCookieService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutCommandHandler(IRefreshTokenCookieService refreshTokenCookieService, IRefreshTokenService refreshTokenService)
        {
            _refreshTokenCookieService = refreshTokenCookieService;
            _refreshTokenService = refreshTokenService;
        }
        public async Task<ApiResponse<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var rawToken = _refreshTokenCookieService.GetRefreshToken();
        
            if (!string.IsNullOrWhiteSpace(rawToken))
            {
                await _refreshTokenService.RevokeAsync(
                    rawToken,
                    cancellationToken);
            }

            _refreshTokenCookieService.DeleteRefreshToken();
        
     
            return new ApiResponse<string>("Logout successful", StatusCodes.Status204NoContent);
        }
    }
}
