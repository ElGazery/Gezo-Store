using Gezo.Application.Common.Settings;
using Gezo.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;



namespace Gezo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings emailSettings;
        public EmailService(IOptions<EmailSettings>options)
        {
            emailSettings = options.Value;
        }
        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
        {
        
            var message = new MimeMessage();
            message.From.Add(
                new MailboxAddress(emailSettings.FromName, emailSettings.FromEmail)
                );
            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;
            message.Body = new BodyBuilder()
            {
                HtmlBody = htmlBody
            }.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(
           emailSettings.Host,
           emailSettings.Port,
           SecureSocketOptions.StartTls,
           cancellationToken);
                await client.AuthenticateAsync(
                    emailSettings.Username,
                    emailSettings.Password,
                    cancellationToken);

                await client.SendAsync(message, cancellationToken);
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(
                        true,
                        cancellationToken);
                }
            }
          
            ;
        }
    }
}
