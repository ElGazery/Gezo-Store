using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.RegisterCommand
{
    public class RegisterCommandVaildator:AbstractValidator<RegisterCommand>
    {
        public RegisterCommandVaildator()
        {
            RuleFor(x => x.FullName)
          .NotEmpty().WithMessage("Name Required")
          .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Required")
                .EmailAddress().WithMessage("Please Write correct Email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Required")
                .MinimumLength(6).WithMessage("Password not less than 6");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Not match the Passwrod");


        }

    }
}
