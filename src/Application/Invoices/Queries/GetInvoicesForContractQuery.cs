using Application.Invoices.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Queries
{
    public record GetInvoicesForContractQuery(
    Guid ContractId,
    Guid CallerId
) : IRequest<OperationResult<IEnumerable<InvoiceDto>>>;
}
