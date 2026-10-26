using Gezo.Application.Common.Settings;
using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly FrontendSettings _frontendSettings;
        private readonly IEmailTemplateService _emailTemplateService;


        public PasswordResetService( UserManager<ApplicationUser> userManager,IEmailService emailService,IOptions<FrontendSettings> frontendOptions, IEmailTemplateService emailTemplateService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _frontendSettings = frontendOptions.Value;
            _emailTemplateService = emailTemplateService;
        }
        public async Task SendResetPasswordEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var baseUrl = _frontendSettings.BaseUrl.TrimEnd('/');
            var resetUrl = $"{baseUrl}/reset-password" +  $"?userId={Uri.EscapeDataString(user.Id)}" + $"&token={Uri.EscapeDataString(token)}";
            var htmlBody = _emailTemplateService.BuildPasswordResetHtml(resetUrl);
            await _emailService.SendAsync(user.Email, "rest you passwrod - Gezo Store", htmlBody,cancellationToken);

        }
    }
}
