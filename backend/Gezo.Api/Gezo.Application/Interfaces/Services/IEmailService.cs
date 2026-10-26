using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IEmailService
    {
       Task SendAsync(
       string to,
       string subject,
       string htmlBody,
       CancellationToken cancellationToken = default);
    }
}
