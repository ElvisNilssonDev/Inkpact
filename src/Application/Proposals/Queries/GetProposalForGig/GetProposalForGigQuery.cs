using Application.Proposals.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Queries.GetProposalForGig
{
    public record GetProposalsForGigQuery(Guid GigId, Guid CallerId) : IRequest<OperationResult<IEnumerable<ProposalDto>>>;
}
