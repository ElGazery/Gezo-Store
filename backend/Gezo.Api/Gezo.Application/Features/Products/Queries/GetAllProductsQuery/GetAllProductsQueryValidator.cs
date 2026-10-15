using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Products.Queries.GetAllProductsQuery
{
    public class GetAllProductsQueryValidator : AbstractValidator<GetAllProductsQuery>
    {
        public GetAllProductsQueryValidator()
        {
            RuleFor(x => x.PageSize).
                GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize).
                InclusiveBetween(1, 100);

            RuleFor(x => x.BrandId)
            .GreaterThan(0)
            .When(x => x.BrandId.HasValue);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue);

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x=>x).
                Must(x => !x.MinPrice.HasValue ||
                          !x.MaxPrice.HasValue ||
                           x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice must be less than or equal to MaxPrice.");

            RuleFor(x => x.Search).
                MaximumLength(100).
                When(x => !string.IsNullOrEmpty(x.Search));

            RuleForEach(x => x.Sort)
               .Must(sort => new[] { "priceAsc", "priceDesc", "nameAsc", "nameDesc" }
               .Contains(sort, StringComparer.OrdinalIgnoreCase))
               .WithMessage("Invalid sort option.");

        }
    }
}
