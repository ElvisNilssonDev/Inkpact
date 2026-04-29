using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.Commands.WithdrawProposal
{
    public record WithdrawProposalCommand(Guid ProposalId, Guid CallerId) : IRequest<OperationResult<bool>>;
}
