using Domain.Common;
using Application.Common.Extensions;
using Domain.Enums;
using Domain.Events;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Commands.CompleteContract
{
    public class CompleteContractHandler : IRequestHandler<CompleteContractCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPublisher _publisher;
        public CompleteContractHandler(IUnitOfWork uow, IPublisher publisher)
        {
            _uow = uow;
            _publisher = publisher;
        }

        public async Task<OperationResult<bool>> Handle(
            CompleteContractCommand request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId && contract.FreelancerId != request.CallerId)
                return "Only the contract participants can complete the contract."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (contract.Status != ContractStatus.Active)
                return "Only active contracts can be completed."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            contract.Status = ContractStatus.Completed;
            contract.EndDate = DateTime.UtcNow;
            contract.UpdatedAt = DateTime.UtcNow;
            _uow.Contracts.Update(contract);

            var gig = await _uow.Gigs.GetByIdAsync(contract.GigId, ct);
            if (gig is not null)
            {
                gig.Status = GigStatus.Completed;
                gig.UpdatedAt = DateTime.UtcNow;
                _uow.Gigs.Update(gig);
            }

            await _uow.SaveChangesAsync(ct);

            await _publisher.Publish(
                new ContractCompletedEvent(contract.Id, contract.ClientId, contract.FreelancerId), ct);

            return true.AsSuccess();
        }
    }
}
