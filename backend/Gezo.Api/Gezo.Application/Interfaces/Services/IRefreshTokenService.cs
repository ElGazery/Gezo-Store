using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IRefreshTokenService
    {
        public string GenerateToken();
        public string HashToken(string token);
        Task<string> CreateAsync( ApplicationUser user,CancellationToken cancellationToken = default);
        Task RevokeAsync(string rawToken, CancellationToken cancellationToken = default);
    }
}
