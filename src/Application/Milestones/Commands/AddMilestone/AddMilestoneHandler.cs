using Application.Common.Extensions;
using Application.Milestones.DTOs;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Commands.AddMilestone
{
    public class AddMilestoneHandler : IRequestHandler<AddMilestoneCommand, OperationResult<MilestoneDto>>
    {
        private readonly IUnitOfWork _uow;

        public AddMilestoneHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<MilestoneDto>> Handle(
            AddMilestoneCommand request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<MilestoneDto>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId)
                return "Only the client can add milestones to a contract."
                    .AsFailure<MilestoneDto>(OperationResultStatus.Unauthorized);

            if (contract.Status != ContractStatus.Active)
                return "Milestones can only be added to active contracts."
                    .AsFailure<MilestoneDto>(OperationResultStatus.Conflict);

            var milestone = new Milestone
            {
                ContractId = request.ContractId,
                Title = request.Title,
                Description = request.Description,
                Amount = request.Amount,
                DueDate = request.DueDate
            };

            await _uow.Milestones.AddAsync(milestone, ct);
            await _uow.SaveChangesAsync(ct);

            var dto = new MilestoneDto(
                milestone.Id,
                milestone.Title,
                milestone.Description,
                milestone.Amount,
                milestone.DueDate,
                milestone.Status,
                milestone.ContractId,
                milestone.CreatedAt
            );

            return dto.AsSuccess();
        }
    }
}
