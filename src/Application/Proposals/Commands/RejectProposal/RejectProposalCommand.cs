using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.RejectProposal
{
    public record RejectProposalCommand(Guid ProposalId, Guid CallerId) : IRequest<OperationResult<bool>>;
}
