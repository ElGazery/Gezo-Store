using Gezo.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Gezo.Infrastructure.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string BuildEmailConfirmationHtml(string confirmationUrl)
        {
           var safeUrl = WebUtility.HtmlEncode(confirmationUrl);

            return $"""
            <div style="font-family: Arial, sans-serif; max-width: 600px; margin: auto;">
                <h2>Welcome to Gezo Store!</h2>

                <p>Thanks for creating your account.</p>

                <p>Please confirm your email address using the link below:</p>

                <a href="{safeUrl}"
                   style="display: inline-block; padding: 12px 20px;
                          background-color: #2563eb; color: white;
                          text-decoration: none; border-radius: 6px;">
                    Confirm Email
                </a>

                <p>If you did not create this account, you can ignore this email.</p>
            </div>
            """;

        }
        public string BuildPasswordResetHtml(string resetUrl)
        {
            var safeUrl = WebUtility.HtmlEncode(resetUrl);

            return $"""
        <div style="font-family: Arial, sans-serif; max-width: 600px; margin: auto;">
            <h2>Reset Your Password</h2>

            <p>We received a request to reset your Gezo Store password.</p>

            <p>Click the button below to choose a new password:</p>

            <a href="{safeUrl}"
               style="display: inline-block; padding: 12px 20px;
                      background-color: #2563eb; color: white;
                      text-decoration: none; border-radius: 6px;">
                Reset Password
            </a>

            <p>If you did not request a password reset, you can safely ignore this email.</p>

            <p>For your security, do not share this link with anyone.</p>
        </div>
        """;
        }
    }
}
