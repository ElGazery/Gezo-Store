using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Gezo.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public CurrentUserService(IHttpContextAccessor http, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = http;
            _userManager = userManager;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public async Task<ApplicationUser> GetCurrentUser()
        {
            var userId = UserId;
            var user =await _userManager.FindByIdAsync(userId);
            return user;
        }

    }
}
