using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.DTOs
{
    public record MilestoneDto(
    Guid Id,
    string Title,
    string Description,
    decimal Amount,
    DateTime DueDate,
    MilestoneStatus Status,
    Guid ContractId,
    DateTime CreatedAt
);
}
