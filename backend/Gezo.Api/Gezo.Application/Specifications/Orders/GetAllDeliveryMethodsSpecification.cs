using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Orders
{
    public class GetAllDeliveryMethodsSpecification :BaseSpecification<DeliveryMethod>
    {
        public GetAllDeliveryMethodsSpecification()
        {
            Criteria = d => d.IsActive == true;
        }
    }
}
