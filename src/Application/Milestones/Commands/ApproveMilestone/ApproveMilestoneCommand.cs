using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Commands.ApproveMilestone
{
    public record ApproveMilestoneCommand(Guid MilestoneId, Guid CallerId) : IRequest<OperationResult<Guid>>;
}
