using Application.Common.Extensions;
using Application.Milestones.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Queries.GetMilestonesForContract
{
    public class GetMilestonesForContractHandler : IRequestHandler<GetMilestonesForContractQuery, OperationResult<IEnumerable<MilestoneDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetMilestonesForContractHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<MilestoneDto>>> Handle(
            GetMilestonesForContractQuery request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<IEnumerable<MilestoneDto>>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId && contract.FreelancerId != request.CallerId)
                return "You are not authorized to view milestones for this contract."
                    .AsFailure<IEnumerable<MilestoneDto>>(OperationResultStatus.Unauthorized);

            var milestones = await _uow.Milestones.GetByContractIdAsync(request.ContractId, ct);

            var dtos = milestones.Select(m => new MilestoneDto(
                m.Id,
                m.Title,
                m.Description,
                m.Amount,
                m.DueDate,
                m.Status,
                m.ContractId,
                m.CreatedAt
            ));

            return dtos.AsSuccess();
        }
    }
}
