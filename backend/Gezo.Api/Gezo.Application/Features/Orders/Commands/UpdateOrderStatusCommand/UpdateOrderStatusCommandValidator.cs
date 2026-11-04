using FluentValidation;

namespace Gezo.Application.Features.Orders.Commands.UpdateOrderStatusCommand
{
    public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid order status");
        }
    }
}

