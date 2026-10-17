using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Brands
{
    public class GetAllBrandsSpecification :BaseSpecification<Brand>
    {
        public GetAllBrandsSpecification()
        {
            Criteria = b => b.IsActive == true;
        }

    }
}
