using Gezo.Application.Common.Settings;
using Gezo.Application.Features.Brands.Queries.GetAllBrandsQuery;
using Gezo.Application.Interfaces.Repositories;
using Gezo.Application.Interfaces.Services;
using Gezo.Application.Specifications.RefreshTokens;
using Gezo.Domain.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Gezo.Infrastructure.Services
{
    public class RefreshTokenService: IRefreshTokenService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService( IUnitOfWork unitOfWork,IOptions<JwtSettings> options)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = options.Value;
        }

        public string GenerateToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return WebEncoders.Base64UrlEncode(randomBytes);
        }

        public string HashToken(string token)
        {
            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var hashBytes = SHA256.HashData(tokenBytes);
            return Convert.ToBase64String(hashBytes);
        }

        public async Task<string> CreateAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var rawToken = GenerateToken();
            var refreshToken = new RefreshToken()
            {
                UserId = user.Id,
                TokenHash = HashToken(rawToken),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays( _jwtSettings.RefreshTokenExpiryInDays)
            };
            await _unitOfWork.GenericRepository<RefreshToken>().AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();
            return rawToken;
        }

        public async Task RevokeAsync(string rawToken, CancellationToken cancellationToken = default)
        {
            var tokenHased = HashToken(rawToken);
            var spec = new RefreshTokenByHashSpecification(tokenHased);
            var token =await _unitOfWork.GenericRepository<RefreshToken>().GetFirstOrDefaultWithSpecAsync(spec, cancellationToken);
            if (token is null || token.RevokedAt is not null)
                return;
            token.RevokedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();
        }
    }

}
