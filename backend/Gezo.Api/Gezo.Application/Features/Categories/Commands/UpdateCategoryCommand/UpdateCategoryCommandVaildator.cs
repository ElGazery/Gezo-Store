using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Commands.UpdateCategoryCommand
{
    public class UpdateCategoryCommandVaildator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandVaildator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Slug)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.ImageUrl)
                .Must(file => file == null || file.Length > 0)
                .WithMessage("Uploaded image must not be empty.")
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Image file size must not exceed 5MB.");
        }
    }
}
