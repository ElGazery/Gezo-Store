using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Orders.Queries.GetAllDeliveryMethods
{
    public class GetAllDeliveryMethodsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public int EstimatedDeliveryDays { get; set; }
    }
}
