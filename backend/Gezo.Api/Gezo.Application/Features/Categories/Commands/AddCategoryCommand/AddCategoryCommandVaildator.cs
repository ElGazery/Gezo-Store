using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Categories.Commands.AddCategoryCommand
{
    public class AddCategoryCommandVaildator:AbstractValidator<AddCategoryCommand>
    {
        public AddCategoryCommandVaildator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);

           
        }
    }
}
