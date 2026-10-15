using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Products.Queries.GetAllProductsQuery
{
    public class GetAllProductsQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }

        public string BrandName { get; set; } = string.Empty;
        public string? MainImageUrl { get; set; }

        public bool IsFeatured { get; set; }
        public bool InStock { get; set; }
    }
}
