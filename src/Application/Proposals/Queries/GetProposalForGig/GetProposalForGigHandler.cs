using Application.Common.Extensions;
using Application.Proposals.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Queries.GetProposalForGig
{
    public class GetProposalsForGigHandler : IRequestHandler<GetProposalsForGigQuery, OperationResult<IEnumerable<ProposalDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetProposalsForGigHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<ProposalDto>>> Handle(
            GetProposalsForGigQuery request,
            CancellationToken ct)
        {
            var gig = await _uow.Gigs.GetByIdAsync(request.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<IEnumerable<ProposalDto>>(OperationResultStatus.NotFound);

            if (gig.ClientId != request.CallerId)
                return "Only the gig owner can view proposals."
                    .AsFailure<IEnumerable<ProposalDto>>(OperationResultStatus.Unauthorized);

            var proposals = await _uow.Proposals.GetByGigIdAsync(request.GigId, ct);

            var dtos = new List<ProposalDto>();
            foreach (var p in proposals)
            {
                var freelancer = await _uow.Users.GetByIdAsync(p.FreelancerId, ct);
                dtos.Add(new ProposalDto(
                    p.Id,
                    p.CoverLetter,
                    p.ProposedRate,
                    p.EstimatedDays,
                    p.Status,
                    gig.Id,
                    gig.Title,
                    p.FreelancerId,
                    freelancer?.Name ?? "Unknown",
                    p.CreatedAt
                ));
            }

            return dtos.AsEnumerable().AsSuccess();
        }
    }
}
