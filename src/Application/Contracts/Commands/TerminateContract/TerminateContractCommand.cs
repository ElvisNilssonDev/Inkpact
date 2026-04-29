using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Commands.TerminateContract
{
    public record TerminateContractCommand(
    Guid ContractId,
    Guid CallerId,
    string Reason
) : IRequest<OperationResult<bool>>;
}
