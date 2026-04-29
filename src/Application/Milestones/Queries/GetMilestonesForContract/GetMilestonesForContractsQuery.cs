using Application.Milestones.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Queries.GetMilestonesForContract
{
    public record GetMilestonesForContractQuery(
    Guid ContractId,
    Guid CallerId
) : IRequest<OperationResult<IEnumerable<MilestoneDto>>>;
}
