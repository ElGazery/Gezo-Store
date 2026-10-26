using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IPasswordResetService
    {
        Task SendResetPasswordEmailAsync(ApplicationUser user, CancellationToken cancellationToken);

    }
}
