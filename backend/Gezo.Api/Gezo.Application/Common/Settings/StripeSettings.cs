using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Common.Settings
{
    public class StripeSettings
    {
        public const string SectionName = "Stripe";

        public string PublishableKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string WebhookSecret { get; set; } = string.Empty;
    }
}
