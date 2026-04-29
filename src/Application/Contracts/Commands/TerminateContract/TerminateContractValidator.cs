using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Commands.TerminateContract
{
    public class TerminateContractValidator : AbstractValidator<TerminateContractCommand>
    {
        public TerminateContractValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.CallerId).NotEmpty();
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("A reason is required when terminating a contract.")
                .MinimumLength(10).WithMessage("Please provide a meaningful reason (at least 10 characters).")
                .MaximumLength(500);
        }
    }
}
