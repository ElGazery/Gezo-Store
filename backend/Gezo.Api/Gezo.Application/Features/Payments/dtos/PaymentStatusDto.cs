using System;

namespace Gezo.Application.Features.Payments.dtos
{
    public class PaymentStatusDto
    {
        public int OrderId { get; set; }

        public string PaymentStatus { get; set; } = string.Empty;

        public string OrderStatus { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;
    }
}

