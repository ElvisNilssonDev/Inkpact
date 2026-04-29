using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.DTOs
{
    public record GigDto(
        Guid Id,
        string Title,
        string Description,
        decimal Budget,
        DateTime Deadline,
        GigStatus Status,
        string Tags,
        Guid ClientId,
        string ClientName,
        DateTime CreatedAt
    );

}
