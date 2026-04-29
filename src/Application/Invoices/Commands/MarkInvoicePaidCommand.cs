using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Commands
{
    public record MarkInvoicePaidCommand(Guid InvoiceId, Guid CallerId) : IRequest<OperationResult<bool>>;
}
