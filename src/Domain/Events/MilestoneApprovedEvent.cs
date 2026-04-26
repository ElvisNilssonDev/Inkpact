using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public record MilestoneApprovedEvent(Guid MilestoneId, Guid ContractId) : INotification;
}
