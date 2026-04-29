using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Commands.CompleteContract
{
    public record CompleteContractCommand(
    Guid ContractId,
    Guid CallerId
    ) : IRequest<OperationResult<bool>>;
}
