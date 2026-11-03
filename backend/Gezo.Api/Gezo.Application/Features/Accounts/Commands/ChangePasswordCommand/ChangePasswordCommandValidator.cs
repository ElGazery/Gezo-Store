using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Application.Features.Accounts.Commands.ChangePasswordCommand
{
    public class ChangePasswordCommandValidator:AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.oldPassword)
                .NotEmpty()
                .WithMessage("old password not be empty");
            RuleFor(x => x.newPassword).
                NotEmpty().WithMessage("new password not be empty").
                Equal(x => x.confirmPassword).WithMessage("passwords must be equles");
            RuleFor(x => x.confirmPassword)
                .NotEmpty().WithMessage("confirm password not be empty");
        }
    }
}
