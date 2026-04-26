using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Events
{
    public record InvoiceOverdueEvent(Guid InvoiceId, Guid ContractId) : INotification;
}
