using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Commands.SubmitMilestone
{
    public record SubmitMilestoneCommand(Guid MilestoneId, Guid CallerId) : IRequest<OperationResult<bool>>;
}
