using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Brands.Commands.UpdateBrandCommand
{
    public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
    {
        public UpdateBrandCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Slug)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Logo)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Uploaded logo must not be empty.")
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Logo file size must not exceed 5MB.");
        }
    }
}

