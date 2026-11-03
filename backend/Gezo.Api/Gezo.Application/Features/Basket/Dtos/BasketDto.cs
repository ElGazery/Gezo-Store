using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Basket.Dtos
{
    public class BasketDto
    {
        public string Id { get; set; } = string.Empty;

        public List<BasketItemDto> Items { get; set; } = new();
    }
}
