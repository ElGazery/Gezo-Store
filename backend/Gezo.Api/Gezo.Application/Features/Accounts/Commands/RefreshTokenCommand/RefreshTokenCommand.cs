using Gezo.ApiResponseBase.Response;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.RefreshTokens;
using Gezo.Domain.Entities;
using Gezo.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.RefreshTokenCommand
{
    public record RefreshTokenCommand :IRequest<ApiResponse<RefreshTokenCommandDto>>
    {
    }

    internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse<RefreshTokenCommandDto>>
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IRefreshTokenCookieService _refreshTokenCookieService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            IRefreshTokenCookieService refreshTokenCookieService,
            IRefreshTokenService refreshTokenService,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ITokenService tokenService)
        {
            _refreshTokenCookieService = refreshTokenCookieService;
            _refreshTokenService = refreshTokenService;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        public async Task<ApiResponse<RefreshTokenCommandDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var currentToken = _refreshTokenCookieService.GetRefreshToken();
            var currentTokenHashed = _refreshTokenService.HashToken(currentToken);
            var spec = new RefreshTokenByHashSpecification(currentTokenHashed);
            var storedToken = await _unitOfWork.GenericRepository<RefreshToken>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
          
            if (storedToken is null)
                throw new UnauthorizedCustomException("You are Unauthorize ");
            if (storedToken.TokenHash != currentTokenHashed)
                throw new UnauthorizedCustomException("You token is not vaild");
            if (storedToken.RevokedAt is not null)
                throw new UnauthorizedCustomException("token not found");
            if (storedToken.ExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedCustomException("Refresh token has expired");
            
            var user = storedToken.User;
            await _refreshTokenService.RevokeAsync(currentToken);
            _refreshTokenCookieService.DeleteRefreshToken();
            var newAccessToken = await _tokenService.GenerateTokenAsync(user);

            var newRefreshToken = await _refreshTokenService.CreateAsync(user);
             _refreshTokenCookieService.SetRefreshToken(newRefreshToken);

            var dto = new RefreshTokenCommandDto() { Token=newAccessToken};

            return new ApiResponse<RefreshTokenCommandDto>(
                dto,
                "Rerfresh Token generated Successfully",
                StatusCodes.Status200OK);

        }
    }

}
