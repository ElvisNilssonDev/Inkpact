using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.RejectProposal
{
    public class RejectProposalHandler : IRequestHandler<RejectProposalCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public RejectProposalHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<bool>> Handle(
            RejectProposalCommand request,
            CancellationToken ct)
        {
            var proposal = await _uow.Proposals.GetByIdAsync(request.ProposalId, ct);
            if (proposal is null)
                return "Proposal not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            var gig = await _uow.Gigs.GetByIdAsync(proposal.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            if (gig.ClientId != request.CallerId)
                return "Only the gig owner can reject proposals."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (proposal.Status != ProposalStatus.Pending)
                return "Only pending proposals can be rejected."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            proposal.Status = ProposalStatus.Rejected;
            proposal.UpdatedAt = DateTime.UtcNow;
            _uow.Proposals.Update(proposal);
            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
