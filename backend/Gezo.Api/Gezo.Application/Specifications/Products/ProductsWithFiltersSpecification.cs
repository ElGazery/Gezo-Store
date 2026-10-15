using System;
using Gezo.Application.Features.Products.Queries.GetAllProductsQuery;
using Gezo.Domain.Entities;

namespace Gezo.Application.Specifications.Products
{
    internal class ProductsWithFiltersSpecification : BaseSpecification<Product>
    {
        public ProductsWithFiltersSpecification(GetAllProductsQuery query)
        {
            //include what i want 
            AddInclude(p => p.Brand);
            AddInclude(p => p.Category);

            //filters as here we can say where(e => ...)

            Criteria = p => (!query.BrandId.HasValue || p.BrandId == query.BrandId.Value) &&
                        (!query.CategoryId.HasValue || p.CategoryId == query.CategoryId.Value) &&
                        (!query.MinPrice.HasValue || p.Price >= query.MinPrice) &&
                        (!query.MaxPrice.HasValue || p.Price <= query.MaxPrice) &&
                        (string.IsNullOrWhiteSpace(query.Search) || p.Name.Contains(query.Search));
            // sorting 

            if (query.Sort != null)
            {
                foreach (var sort in query.Sort)
                {
                    switch (sort.ToLowerInvariant())
                    {
                        case "priceasc":
                            AddOrderBy(p => p.Price);
                            break;

                        case "pricedesc":
                            AddOrderBy(p => p.Price, true);
                            break;

                        case "nameasc":
                            AddOrderBy(p => p.Name);
                            break;

                        case "namedesc":
                            AddOrderBy(p => p.Name, true);
                            break;
                    }
                }
            }
            // pagination

            ApplyPagination((query.PageIndex - 1) * query.PageSize, query.PageSize);
        }
    }
}