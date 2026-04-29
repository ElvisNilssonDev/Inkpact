using Application.Proposals.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.SubmitProposal
{
    public record SubmitProposalCommand(
    Guid GigId,
    Guid FreelancerId,
    string CoverLetter,
    decimal ProposedRate,
    int EstimatedDays
    ) : IRequest<OperationResult<ProposalDto>>;
}
