using Application.Proposals.Commands.WithdrawProposal;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is Required.")
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required");            

        }
    }
}
