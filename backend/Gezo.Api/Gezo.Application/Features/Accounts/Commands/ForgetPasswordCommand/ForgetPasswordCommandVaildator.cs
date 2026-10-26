using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.ForgetPasswordCommand
{
    public class ForgetPasswordCommandVaildator :AbstractValidator<ForgetPasswordCommand>
    {
        public ForgetPasswordCommandVaildator()
        {
            RuleFor(e => e.email).NotEmpty().WithMessage("email not be empty").
                EmailAddress().WithMessage("write vaild email ");

        }
    }
}
