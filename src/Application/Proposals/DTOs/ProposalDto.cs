using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Proposals.DTOs
{
    public record ProposalDto(
    Guid Id,
    string CoverLetter,
    decimal ProposedRate,
    int EstimatedDays,
    ProposalStatus Status,
    Guid GigId,
    string GigTitle,
    Guid FreelancerId,
    string FreelancerName,
    DateTime CreatedAt
    );
}
