using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.WithdrawProposal
{
    public class WithdrawProposalHandler : IRequestHandler<WithdrawProposalCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public WithdrawProposalHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<bool>> Handle(
            WithdrawProposalCommand request,
            CancellationToken ct)
        {
            var proposal = await _uow.Proposals.GetByIdAsync(request.ProposalId, ct);
            if (proposal is null)
                return "Proposal not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            if (proposal.FreelancerId != request.CallerId)
                return "Only the proposal owner can withdraw."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (proposal.Status != ProposalStatus.Pending)
                return "Only pending proposals can be withdrawn."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            proposal.Status = ProposalStatus.Withdrawn;
            proposal.UpdatedAt = DateTime.UtcNow;
            _uow.Proposals.Update(proposal);
            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
