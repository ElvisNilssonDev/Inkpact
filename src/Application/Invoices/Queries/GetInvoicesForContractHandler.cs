using Application.Common.Extensions;
using Application.Invoices.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Invoices.Queries
{
    public class GetInvoicesForContractHandler : IRequestHandler<GetInvoicesForContractQuery, OperationResult<IEnumerable<InvoiceDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetInvoicesForContractHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<InvoiceDto>>> Handle(
            GetInvoicesForContractQuery request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<IEnumerable<InvoiceDto>>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId && contract.FreelancerId != request.CallerId)
                return "You are not authorized to view invoices for this contract."
                    .AsFailure<IEnumerable<InvoiceDto>>(OperationResultStatus.Unauthorized);

            var invoices = await _uow.Invoices.GetByContractIdAsync(request.ContractId, ct);

            var dtos = invoices.Select(i => new InvoiceDto(
                i.Id,
                i.IssuedAt,
                i.DueDate,
                i.TotalAmount,
                i.Status,
                i.ContractId,
                i.LineItems.Select(li => new InvoiceLineItemDto(
                    li.Id,
                    li.Description,
                    li.Amount,
                    li.MilestoneId
                )),
                i.CreatedAt
            ));

            return dtos.AsSuccess();
        }
    }
}
