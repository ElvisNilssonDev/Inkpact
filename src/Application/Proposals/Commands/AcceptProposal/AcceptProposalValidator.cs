using FluentValidation;

namespace Application.Proposals.Commands.AcceptProposal
{
    public class AcceptProposalValidator : AbstractValidator<AcceptProposalCommand>
    {
        public AcceptProposalValidator()
        {
            RuleFor(x => x.ProposalId).NotEmpty();
            RuleFor(x => x.CallerId).NotEmpty();
        }
    }
}