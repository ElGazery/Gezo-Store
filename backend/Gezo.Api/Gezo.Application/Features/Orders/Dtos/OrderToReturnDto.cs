using Gezo.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Gezo.Application.Features.Orders.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string PaymentStatus { get; set; } = string.Empty;

        public int DeliveryMethodId { get; set; }

        public string DeliveryMethodName { get; set; } = string.Empty;
        public PaymentMethod PaymentMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        public string ShipToName { get; set; } = string.Empty;

        public string ShipToPhone { get; set; } = string.Empty;

        public string ShipToAddress { get; set; } = string.Empty;

        public string ShipToCity { get; set; } = string.Empty;

        public string? ShipToPostalCode { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }

        public IReadOnlyList<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}

