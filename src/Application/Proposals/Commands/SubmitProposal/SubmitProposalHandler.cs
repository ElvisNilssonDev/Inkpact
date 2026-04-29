using Application.Common.Extensions;
using Application.Proposals.DTOs;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;

namespace Application.Proposals.Commands.SubmitProposal
{
    public class SubmitProposalHandler : IRequestHandler<SubmitProposalCommand, OperationResult<ProposalDto>>
    {
        private readonly IUnitOfWork _uow;

        public SubmitProposalHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<ProposalDto>> Handle(
            SubmitProposalCommand request,
            CancellationToken ct)
        {
            var freelancer = await _uow.Users.GetByIdAsync(request.FreelancerId, ct);
            if (freelancer is null || freelancer.Role != UserRole.Freelancer)
                return "Only freelancers can submit proposals."
                    .AsFailure<ProposalDto>(OperationResultStatus.Unauthorized);

            var gig = await _uow.Gigs.GetByIdAsync(request.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<ProposalDto>(OperationResultStatus.NotFound);

            if (gig.Status != GigStatus.Open)
                return "Proposals can only be submitted to open gigs."
                    .AsFailure<ProposalDto>(OperationResultStatus.Conflict);

            var existing = await _uow.Proposals.GetByGigIdAsync(request.GigId, ct);
            if (existing.Any(p => p.FreelancerId == request.FreelancerId))
                return "You have already submitted a proposal for this gig."
                    .AsFailure<ProposalDto>(OperationResultStatus.Conflict);

            var proposal = new Proposal
            {
                GigId = request.GigId,
                FreelancerId = request.FreelancerId,
                CoverLetter = request.CoverLetter,
                ProposedRate = request.ProposedRate,
                EstimatedDays = request.EstimatedDays
            };

            await _uow.Proposals.AddAsync(proposal, ct);
            await _uow.SaveChangesAsync(ct);

            var dto = new ProposalDto(
                proposal.Id,
                proposal.CoverLetter,
                proposal.ProposedRate,
                proposal.EstimatedDays,
                proposal.Status,
                gig.Id,
                gig.Title,
                freelancer.Id,
                freelancer.Name,
                proposal.CreatedAt
            );

            return dto.AsSuccess();
        }
    }
}
