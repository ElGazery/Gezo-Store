using Gezo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Specifications.Categories
{
    public class GetAllCategoriesSpacification :BaseSpecification<Category>
    {
        public GetAllCategoriesSpacification()
        {
            Criteria = c => c.IsActive == true;
        
        }


       

    }
}
