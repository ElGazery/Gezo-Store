using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Gezo.Application.Specifications.Products
{
    public class ProductCountSpecification:BaseSpecification<Product>
    {
        public ProductCountSpecification(Expression<Func<Product, bool>>? expression)
        {
            Criteria = expression;

        }


    }
}
