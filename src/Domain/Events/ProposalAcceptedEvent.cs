using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace Domain.Events
{
    public record ProposalAcceptedEvent(Guid ProposalId, Guid GigId, Guid FreelancerId) : INotification;
}
