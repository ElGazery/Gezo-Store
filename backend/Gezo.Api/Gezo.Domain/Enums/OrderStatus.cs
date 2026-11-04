using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,
        PaymentReceived,
        PaymentFailed,
        Shipped,
        Delivered,
        Cancelled
    }
}
