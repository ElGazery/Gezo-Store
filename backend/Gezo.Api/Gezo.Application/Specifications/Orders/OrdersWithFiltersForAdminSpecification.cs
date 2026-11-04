using Gezo.Application.Features.Orders.Queries.GetAllOrdersForAdminQuery;
using Gezo.Domain.Entities;
using System;

namespace Gezo.Application.Specifications.Orders
{
    public class OrdersWithFiltersForAdminSpecification : BaseSpecification<Order>
    {
        public OrdersWithFiltersForAdminSpecification(GetAllOrdersForAdminQuery query)
        {
            AddInclude(o => o.Items);

            Criteria = o => (!query.Status.HasValue || o.Status == query.Status.Value) &&
                            (!query.PaymentStatus.HasValue || o.PaymentStatus == query.PaymentStatus.Value) &&
                            (string.IsNullOrWhiteSpace(query.Search) || o.OrderNumber.Contains(query.Search) || o.ShipToName.Contains(query.Search));

            AddOrderBy(o => o.OrderDate, true);

            ApplyPagination((query.PageIndex - 1) * query.PageSize, query.PageSize);
        }
    }
}

