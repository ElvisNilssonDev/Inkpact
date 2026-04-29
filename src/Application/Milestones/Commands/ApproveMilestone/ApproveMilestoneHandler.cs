using Application.Common.Extensions;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Commands.ApproveMilestone
{
    public class ApproveMilestoneHandler : IRequestHandler<ApproveMilestoneCommand, OperationResult<Guid>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPublisher _publisher;

        public ApproveMilestoneHandler(IUnitOfWork uow, IPublisher publisher)
        {
            _uow = uow;
            _publisher = publisher;
        }

        public async Task<OperationResult<Guid>> Handle(
            ApproveMilestoneCommand request,
            CancellationToken ct)
        {
            var milestone = await _uow.Milestones.GetByIdAsync(request.MilestoneId, ct);
            if (milestone is null)
                return "Milestone not found.".AsFailure<Guid>(OperationResultStatus.NotFound);

            var contract = await _uow.Contracts.GetByIdAsync(milestone.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<Guid>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId)
                return "Only the client can approve milestones."
                    .AsFailure<Guid>(OperationResultStatus.Unauthorized);

            if (milestone.Status != MilestoneStatus.Submitted)
                return "Only submitted milestones can be approved."
                    .AsFailure<Guid>(OperationResultStatus.Conflict);

            // 1. Update milestone
            milestone.Status = MilestoneStatus.Approved;
            milestone.UpdatedAt = DateTime.UtcNow;
            _uow.Milestones.Update(milestone);

            // 2. Auto-generate invoice
            var invoice = new Invoice
            {
                ContractId = contract.Id,
                DueDate = DateTime.UtcNow.AddDays(14),  // 14-day payment terms
                TotalAmount = milestone.Amount,
                Status = InvoiceStatus.Sent
            };
            await _uow.Invoices.AddAsync(invoice, ct);

            // 3. Add line item for the milestone
            var lineItem = new InvoiceLineItem
            {
                InvoiceId = invoice.Id,
                MilestoneId = milestone.Id,
                Description = milestone.Title,
                Amount = milestone.Amount
            };
            invoice.LineItems.Add(lineItem);

            // 4. Save everything in one transaction
            await _uow.SaveChangesAsync(ct);

            // 5. Publish event AFTER save succeeds
            await _publisher.Publish(
                new MilestoneApprovedEvent(milestone.Id, contract.Id), ct);

            return invoice.Id.AsSuccess();
        }
    }
}
