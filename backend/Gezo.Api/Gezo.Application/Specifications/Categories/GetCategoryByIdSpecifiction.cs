using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Categories
{
    public class GetCategoryByIdSpecifiction:BaseSpecification<Category>
    {
        public GetCategoryByIdSpecifiction(int id)
        {
            Criteria = p => p.IsActive && p.Id == id;
            AddInclude(p => p.Products.Where(product => product.IsActive));
        }
    }
}
