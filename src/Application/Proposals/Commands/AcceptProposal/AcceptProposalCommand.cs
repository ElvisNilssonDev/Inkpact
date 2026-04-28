using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.AcceptProposal
{
    public record AcceptProposalCommand(
    Guid ProposalId,
    Guid CallerId
    ) : IRequest<OperationResult<Guid>>;
}
