using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Interfaces.Services
{
    public interface IEmailTemplateService
    {
         string BuildEmailConfirmationHtml(string confirmationUrl);
    }
}
