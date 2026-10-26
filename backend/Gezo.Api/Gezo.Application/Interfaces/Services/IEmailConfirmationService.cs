using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IEmailConfirmationService
    {
        Task SendConfirmationEmailAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default);
    }
}
