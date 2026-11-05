using Gezo.Domain.Entities;
using System;

namespace Gezo.Application.Specifications.Orders
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
        {
            Criteria = o => o.PaymentIntentId == paymentIntentId;
        }
    }
}

