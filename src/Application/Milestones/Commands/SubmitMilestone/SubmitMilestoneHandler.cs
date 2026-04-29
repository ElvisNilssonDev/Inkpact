using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Commands.SubmitMilestone
{
    public class SubmitMilestoneHandler : IRequestHandler<SubmitMilestoneCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public SubmitMilestoneHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<bool>> Handle(
            SubmitMilestoneCommand request,
            CancellationToken ct)
        {
            var milestone = await _uow.Milestones.GetByIdAsync(request.MilestoneId, ct);
            if (milestone is null)
                return "Milestone not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            var contract = await _uow.Contracts.GetByIdAsync(milestone.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            if (contract.FreelancerId != request.CallerId)
                return "Only the freelancer can submit milestones."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (milestone.Status != MilestoneStatus.Pending)
                return "Only pending milestones can be submitted."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            milestone.Status = MilestoneStatus.Submitted;
            milestone.UpdatedAt = DateTime.UtcNow;
            _uow.Milestones.Update(milestone);
            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
