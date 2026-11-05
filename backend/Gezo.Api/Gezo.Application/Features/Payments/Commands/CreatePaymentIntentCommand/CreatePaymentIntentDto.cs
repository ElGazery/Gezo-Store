using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Payments.Commands.CreatePaymentIntentCommand
{
    public class CreatePaymentIntentDto
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
