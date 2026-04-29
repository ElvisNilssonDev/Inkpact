using Application.Common.Extensions;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Commands
{
    public class MarkInvoicePaidHandler : IRequestHandler<MarkInvoicePaidCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public MarkInvoicePaidHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<bool>> Handle(
            MarkInvoicePaidCommand request,
            CancellationToken ct)
        {
            var invoice = await _uow.Invoices.GetByIdAsync(request.InvoiceId, ct);
            if (invoice is null)
                return "Invoice not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            var contract = await _uow.Contracts.GetByIdAsync(invoice.ContractId, ct);
            if (contract is null)
                return "Contract not found".AsFailure<bool>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId)
                return "Only the client can mark an invoice as paid.".AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (invoice.Status == Domain.Enums.InvoiceStatus.Paid)
                return "Invoice is already paid.".AsFailure<bool>(OperationResultStatus.Conflict);

            invoice.Status = Domain.Enums.InvoiceStatus.Paid;
            invoice.UpdatedAt = DateTime.Now;
            _uow.Invoices.Update(invoice);
            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
