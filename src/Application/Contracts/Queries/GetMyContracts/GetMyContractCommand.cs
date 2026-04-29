using Application.Contracts.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Queries.GetMyContracts
{
    public record GetMyContractsQuery(Guid UserId) : IRequest<OperationResult<IEnumerable<ContractDto>>>;
}
