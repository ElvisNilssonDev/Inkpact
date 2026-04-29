using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public record ContractCompletedEvent(Guid ContractId, Guid ClientId, Guid FreelancerId) : INotification;
}
