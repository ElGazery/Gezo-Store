using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Payments.dtos
{
    public record PaymentIntentResultDto(string PaymentIntentId,string ClientSecret,string Status);
}
