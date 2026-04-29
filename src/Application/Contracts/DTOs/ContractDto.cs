using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.DTOs
{
    public record ContractDto(
    Guid Id,
    decimal AgreedRate,
    DateTime StartDate,
    DateTime? EndDate,
    ContractStatus Status,
    string? TerminationReason,
    DateTime? TerminatedAt,
    Guid GigId,
    string GigTitle,
    Guid ClientId,
    string ClientName,
    Guid FreelancerId,
    string FreelancerName,
    DateTime CreatedAt
    );
}
