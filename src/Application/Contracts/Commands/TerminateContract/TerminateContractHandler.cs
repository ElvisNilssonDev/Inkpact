using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Commands.TerminateContract
{
    public class TerminateContractHandler : IRequestHandler<TerminateContractCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public TerminateContractHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<bool>> Handle(
            TerminateContractCommand request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId && contract.FreelancerId != request.CallerId)
                return "Only the contract participants can terminate the contract."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (contract.Status != ContractStatus.Active)
                return "Only active contracts can be terminated."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            contract.Status = ContractStatus.Terminated;
            contract.EndDate = DateTime.UtcNow;
            contract.TerminatedAt = DateTime.Now;
            contract.TerminationReason = request.Reason;
            contract.UpdatedAt = DateTime.UtcNow;
            _uow.Contracts.Update(contract);

            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
