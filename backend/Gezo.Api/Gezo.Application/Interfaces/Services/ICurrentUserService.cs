using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        Task<ApplicationUser> GetCurrentUser();
    }
}
