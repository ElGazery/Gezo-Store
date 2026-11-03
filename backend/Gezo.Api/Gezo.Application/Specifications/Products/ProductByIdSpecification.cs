using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Products
{
    public class ProductByIdSpecification:BaseSpecification<Product>
    {
        public ProductByIdSpecification(int id)
        {
            Criteria = p =>  p.IsActive == true && p.Id==id;
            AddInclude(p => p.Images);

        }
    }
}
