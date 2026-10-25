using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IRefreshTokenCookieService
    {
        string? GetRefreshToken();
        void SetRefreshToken(string token);

        void DeleteRefreshToken();
    }
}
