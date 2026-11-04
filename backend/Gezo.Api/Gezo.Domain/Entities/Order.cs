using Gezo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        public int DeliveryMethodId { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; } = null!;

        public string DeliveryMethodName { get; set; } = string.Empty;

        public decimal DeliveryMethodCost { get; set; }

        // Shipping address snapshot
        public string ShipToName { get; set; } = string.Empty;

        public string ShipToPhone { get; set; } = string.Empty;

        public string ShipToAddress { get; set; } = string.Empty;

        public string ShipToCity { get; set; } = string.Empty;

        public string? ShipToPostalCode { get; set; }

        public decimal Subtotal { get; set; } = 0;

        public decimal Total { get; set; } = 0;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
