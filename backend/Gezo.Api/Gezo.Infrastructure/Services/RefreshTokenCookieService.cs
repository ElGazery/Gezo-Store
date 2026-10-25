using Gezo.Application.Common.Settings;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Services
{
    public class RefreshTokenCookieService : IRefreshTokenCookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtSettings _jwtSettings;
        private const string CookieName = "refreshToken";
        private const string CookiePath = "/api/Account";

        public RefreshTokenCookieService( IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtSettings = jwtOptions.Value;
        }
        public void SetRefreshToken(string token)
        {
            var context = _httpContextAccessor.HttpContext
            ?? throw new UnauthorizedCustomException(  "HTTP context is not available.");

            context.Response.Cookies.Append(CookieName, token, new CookieOptions()
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires=DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays),
                Path = CookiePath
            });
        }
        public void DeleteRefreshToken()
        {
            var context = _httpContextAccessor.HttpContext
            ?? throw new UnauthorizedCustomException("HTTP context is not available.");

            context.Response.Cookies.Delete(
            CookieName,new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Path = CookiePath
            });

        }

        public string? GetRefreshToken()
        {
            var context = _httpContextAccessor.HttpContext
           ?? throw new UnauthorizedCustomException("HTTP context is not available.");

            return context.Request.
                Cookies.
                TryGetValue(CookieName, out var token) ? token : null;
        }
    }
}
