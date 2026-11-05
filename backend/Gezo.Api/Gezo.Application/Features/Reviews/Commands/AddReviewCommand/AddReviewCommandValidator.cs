using FluentValidation;

namespace Gezo.Application.Features.Reviews.Commands.AddReviewCommand
{
    public class AddReviewCommandValidator : AbstractValidator<AddReviewCommand>
    {
        public AddReviewCommandValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required")
                .MaximumLength(500).WithMessage("Comment must not exceed 500 characters");
        }
    }
}

