using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Domain.Enums;

namespace Application.Auth.Commands.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is Required.")
                .MaximumLength(45);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is Required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required")
                .MinimumLength(8).WithMessage("Minimum of 8 characters.")
                .Matches(@"[A-Za-z]").WithMessage("Password must contain at least one letter.")
                .Matches(@"\d").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.Role)
                .Must(r => r == UserRole.Client || r == UserRole.Freelancer)
                .WithMessage("Role must be either Client or Freelancer.");

        }
    }
}
