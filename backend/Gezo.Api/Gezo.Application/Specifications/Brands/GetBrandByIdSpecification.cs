using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Brands
{
    public class GetBrandByIdSpecification : BaseSpecification<Brand>
    {
        public GetBrandByIdSpecification(int id)
        {
            Criteria = b => b.IsActive && b.Id == id;
            AddInclude(b => b.Products.Where(product => product.IsActive));
        }
    }
}

