using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Products.Commands.UpdateProductCommand
{
    public class UpdateProductCommandVaildator:AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandVaildator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.Price)
                .GreaterThan(0);
            RuleFor(x => x.BrandId)
                .NotEmpty()
                .GreaterThan(0);
            RuleFor(x => x.CategoryId)
              .NotEmpty()
              .GreaterThan(0);
            RuleFor(x => x.DiscountPrice)
                .GreaterThan(0)
                .LessThan(x => x.Price)
                .When(x => x.DiscountPrice.HasValue);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.SKU)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.NewImages)
                .NotNull()
                .Must(images => images.Count <= 5)
                .WithMessage("You can upload a maximum of 5 images.")
                .Must(images => images.All(image =>
                     image.Length<5 ))
                .WithMessage("All uploaded images must be valid and non-empty.");

        }
    }
}
