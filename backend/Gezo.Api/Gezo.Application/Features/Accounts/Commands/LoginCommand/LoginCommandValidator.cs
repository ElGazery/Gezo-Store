using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.LoginCommand
{
    public class LoginCommandValidator: AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.email).
                NotEmpty().
                WithMessage("Email Not be Empty").
                EmailAddress().
                WithMessage("Please enter a valid email address");

            RuleFor(x => x.password).
                NotEmpty().
                WithMessage("Password not be empty");

        }
    }
}
