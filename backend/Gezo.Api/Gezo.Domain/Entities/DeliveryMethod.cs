using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Domain.Entities
{
    public class DeliveryMethod
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public int EstimatedDeliveryDays { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
