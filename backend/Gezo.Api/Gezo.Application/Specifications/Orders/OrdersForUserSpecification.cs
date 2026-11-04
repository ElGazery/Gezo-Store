using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Orders
{
    public class OrdersForUserSpecification : BaseSpecification<Order>
    {
        public OrdersForUserSpecification(string userId)
        {
            Criteria = o => o.UserId == userId;
            AddInclude(o => o.Items);
            AddOrderBy(o => o.OrderDate, true);
        }
    }
}

