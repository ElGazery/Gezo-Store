using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Basket.Dtos
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? PictureUrl { get; set; }
        public int Quantity { get; set; }
    }
}

