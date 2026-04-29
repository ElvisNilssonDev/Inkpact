using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Domain.Events
{
    public record ContractCreatedEvent(Guid ContractId, Guid ClientId, Guid FreelancerId) : INotification;
}
