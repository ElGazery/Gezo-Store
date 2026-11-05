using Gezo.Domain.Entities;
using System;

namespace Gezo.Application.Specifications.Reviews
{
    public class GetProductReviewsSpecification : BaseSpecification<Review>
    {
        public GetProductReviewsSpecification(int productId)
        {
            Criteria = r => r.ProductId == productId;
            AddInclude(r => r.User);
            AddOrderBy(r => r.CreatedAt, true);
        }
    }
}

