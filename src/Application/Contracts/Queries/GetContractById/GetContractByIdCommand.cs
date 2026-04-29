using Application.Contracts.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Queries.GetContractById
{
    public record GetContractByIdQuery(Guid ContractId, Guid CallerId) : IRequest<OperationResult<ContractDto>>;
}
