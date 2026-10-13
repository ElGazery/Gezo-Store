using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Products.Queries.GetProdcutByIdQuery
{
    public class GetProductByIdQueryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }

        public int StockQuantity { get; set; }
        public string SKU { get; set; }
        public string Slug { get; set; }

        public string BrandName { get; set; }
        public string CategoryName { get; set; }

        public List<string> Images { get; set; }

    }
}
