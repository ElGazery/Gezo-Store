using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Orders
{
    public class OrderWithItemsByIdAndUserSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsByIdAndUserSpecification(int id, string userId)
        {
            Criteria = o => o.Id == id && o.UserId == userId;
            AddInclude(o => o.Items);
        }
    }
}

