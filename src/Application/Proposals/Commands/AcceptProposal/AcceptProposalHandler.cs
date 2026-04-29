using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.AcceptProposal
{
    public class AcceptProposalHandler : IRequestHandler<AcceptProposalCommand, OperationResult<Guid>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPublisher _publisher;

        public AcceptProposalHandler(IUnitOfWork uow, IPublisher publisher)
        {
            _uow = uow;
            _publisher = publisher;
        }

        public async Task<OperationResult<Guid>> Handle(
            AcceptProposalCommand request,
            CancellationToken ct)
        {
            var proposal = await _uow.Proposals.GetByIdAsync(request.ProposalId, ct);
            if (proposal is null)
                return "Proposal not found.".AsFailure<Guid>(OperationResultStatus.NotFound);

            var gig = await _uow.Gigs.GetByIdAsync(proposal.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<Guid>(OperationResultStatus.NotFound);

            if (gig.ClientId != request.CallerId)
                return "Only the gig owner can accept proposals."
                    .AsFailure<Guid>(OperationResultStatus.Unauthorized);

            if (proposal.Status != ProposalStatus.Pending)
                return "Only pending proposals can be accepted."
                    .AsFailure<Guid>(OperationResultStatus.Conflict);

            if (gig.Status != GigStatus.Open)
                return "Proposals can only be accepted on open gigs."
                    .AsFailure<Guid>(OperationResultStatus.Conflict);

            // 1. Update proposal
            proposal.Status = ProposalStatus.Accepted;
            proposal.UpdatedAt = DateTime.UtcNow;
            _uow.Proposals.Update(proposal);

            // 2. Update gig
            gig.Status = GigStatus.InProgress;
            gig.UpdatedAt = DateTime.UtcNow;
            _uow.Gigs.Update(gig);

            // 3. Create contract
            var contract = new Contract
            {
                GigId = gig.Id,
                ClientId = gig.ClientId,
                FreelancerId = proposal.FreelancerId,
                AgreedRate = proposal.ProposedRate,
                StartDate = DateTime.UtcNow
            };
            await _uow.Contracts.AddAsync(contract, ct);

            // 4. Reject all other proposals on this gig
            await _uow.Proposals.RejectAllExceptAsync(gig.Id, proposal.Id, ct);

            // 5. Save everything in one transaction
            await _uow.SaveChangesAsync(ct);

            // 6. Publish domain events AFTER saving succeeds
            await _publisher.Publish(
                new ProposalAcceptedEvent(proposal.Id, gig.Id, proposal.FreelancerId), ct);
            await _publisher.Publish(
                new ContractCreatedEvent(contract.Id, gig.ClientId, proposal.FreelancerId), ct);

            return contract.Id.AsSuccess();
        }
    }
}
