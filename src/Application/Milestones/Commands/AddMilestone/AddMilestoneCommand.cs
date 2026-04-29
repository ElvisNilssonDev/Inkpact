using Application.Milestones.DTOs;
using Domain.Common;
using MediatR;

namespace Application.Milestones.Commands.AddMilestone
{
    public record AddMilestoneCommand(
    Guid ContractId,
    Guid CallerId,
    string Title,
    string Description,
    decimal Amount,
    DateTime DueDate
) : IRequest<OperationResult<MilestoneDto>>;
}
