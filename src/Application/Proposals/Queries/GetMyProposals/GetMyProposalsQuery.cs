using Application.Proposals.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Queries.GetMyProposals
{
    public record GetMyProposalsQuery(Guid FreelancerId) : IRequest<OperationResult<IEnumerable<ProposalDto>>>;

}
