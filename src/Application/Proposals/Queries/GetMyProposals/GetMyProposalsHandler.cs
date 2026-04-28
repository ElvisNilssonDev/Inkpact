using Application.Common.Extensions;
using Application.Proposals.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Queries.GetMyProposals
{
    public class GetMyProposalsHandler : IRequestHandler<GetMyProposalsQuery, OperationResult<IEnumerable<ProposalDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetMyProposalsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<ProposalDto>>> Handle(
            GetMyProposalsQuery request,
            CancellationToken ct)
        {
            var freelancer = await _uow.Users.GetByIdAsync(request.FreelancerId, ct);
            if (freelancer is null)
                return "Freelancer not found.".AsFailure<IEnumerable<ProposalDto>>(OperationResultStatus.NotFound);

            var proposals = await _uow.Proposals.GetByFreelancerIdAsync(request.FreelancerId, ct);

            var dtos = new List<ProposalDto>();
            foreach (var p in proposals)
            {
                var gig = await _uow.Gigs.GetByIdAsync(p.GigId, ct);
                dtos.Add(new ProposalDto(
                    p.Id,
                    p.CoverLetter,
                    p.ProposedRate,
                    p.EstimatedDays,
                    p.Status,
                    p.GigId,
                    gig?.Title ?? "Unknown gig",
                    freelancer.Id,
                    freelancer.Name,
                    p.CreatedAt
                ));
            }

            return dtos.AsEnumerable().AsSuccess();
        }
    }
}
