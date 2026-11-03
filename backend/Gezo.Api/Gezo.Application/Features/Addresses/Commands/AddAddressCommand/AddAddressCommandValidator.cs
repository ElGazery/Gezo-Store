using FluentValidation;

namespace Gezo.Application.Features.Addresses.Commands.AddAddressCommand
{
    public class AddAddressCommandValidator : AbstractValidator<AddAddressCommand>
    {
        public AddAddressCommandValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(150).WithMessage("Street must not exceed 150 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x.Governorate)
                .NotEmpty().WithMessage("Governorate is required")
                .MaximumLength(100).WithMessage("Governorate must not exceed 100 characters");

            RuleFor(x => x.PostalCode)
                .GreaterThan(0).When(x => x.PostalCode.HasValue)
                .WithMessage("Postal code must be greater than 0");
        }
    }
}

