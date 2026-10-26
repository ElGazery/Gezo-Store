using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.ResetPasswordCommand
{
    public class ResetPasswordCommandvaildator :AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandvaildator()
        {
            RuleFor(e => e.userId).NotEmpty().WithMessage("userId not be empty");
            RuleFor(e => e.token).NotEmpty().WithMessage("token not be empty");
            RuleFor(e => e.newPassword).NotEmpty().WithMessage("password not be empty").MinimumLength(8);


        }
    }
}
