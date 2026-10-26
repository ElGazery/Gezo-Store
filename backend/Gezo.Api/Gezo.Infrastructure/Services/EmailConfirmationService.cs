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
    public class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly FrontendSettings _frontendSettings;

        public EmailConfirmationService( 
            UserManager<ApplicationUser> userManager,  
            IEmailService emailService, 
            IEmailTemplateService emailTemplateService,
            IOptions<FrontendSettings> frontendOptions)
        {
            _userManager = userManager;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _frontendSettings = frontendOptions.Value;
        }
        public async Task SendConfirmationEmailAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var baseUrl = _frontendSettings.BaseUrl.TrimEnd('/');
            var confirmationUrl = $"{baseUrl}/confirm-email" + $"?userId={Uri.EscapeDataString(user.Id)}" +$"&token={Uri.EscapeDataString(token)}";
            var htmlBody = _emailTemplateService.BuildEmailConfirmationHtml(confirmationUrl);
            await _emailService.SendAsync( user.Email,
            "Confirm Your Email - Gezo Store",
            htmlBody,
            cancellationToken);

        }
    }
}
