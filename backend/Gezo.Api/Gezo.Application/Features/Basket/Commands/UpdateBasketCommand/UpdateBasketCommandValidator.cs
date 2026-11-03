using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Basket.Commands.UpdateBasketCommand
{
    public class UpdateBasketCommandValidator:AbstractValidator<UpdateBasketCommand>
    {
        public UpdateBasketCommandValidator()
        {
          
            RuleFor(x => x.productId)
                .GreaterThan(0);

            RuleFor(x => x.quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(99);

        }
    }
}
