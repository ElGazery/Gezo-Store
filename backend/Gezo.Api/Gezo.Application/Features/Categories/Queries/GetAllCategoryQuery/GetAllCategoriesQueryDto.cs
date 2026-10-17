using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Queries.GetAllCategoryQuery
{
    public class GetAllCategoriesQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string? ImageUrl { get; set; }
    }
}
