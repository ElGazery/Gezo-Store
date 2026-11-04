using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Orders.Commands.AddOrderCommand
{
    public class AddOrderCommandvalidator :AbstractValidator<AddOrderCommand>
    {
        public AddOrderCommandvalidator()
        {
            RuleFor(x => x.DeliveryMethodId).GreaterThan(0);

            RuleFor(x => x.ShipToName).NotEmpty().MaximumLength(100);

            RuleFor(x => x.ShipToPhone).NotEmpty().MaximumLength(20);  


            RuleFor(x => x.ShipToAddress).NotEmpty().MaximumLength(250);

            RuleFor(x => x.ShipToCity).NotEmpty().MaximumLength(100);

            RuleFor(x => x.ShipToPostalCode).MaximumLength(10);
        }
    }
}
