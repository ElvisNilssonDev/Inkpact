using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.SubmitProposal
{
    public class SubmitProposalValidator : AbstractValidator<SubmitProposalCommand>
    {
        public SubmitProposalValidator()
        {
            RuleFor(x => x.GigId).NotEmpty();
            RuleFor(x => x.FreelancerId).NotEmpty();

            RuleFor(x => x.CoverLetter)
                .NotEmpty().WithMessage("Cover letter is required.")
                .MinimumLength(50).WithMessage("Cover letter must be at least 50 characters.")
                .MaximumLength(2000);

            RuleFor(x => x.ProposedRate)
                .GreaterThan(0).WithMessage("Proposed rate must be greater than 0.");

            RuleFor(x => x.EstimatedDays)
                .GreaterThan(0).WithMessage("Estimated days must be greater than 0.")
                .LessThanOrEqualTo(365).WithMessage("Estimated days cannot exceed 365.");
        }
    }
}
