using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reviews.DTOs
{
    public record ReviewDto(
    Guid Id,
    int Rating,
    string Comment,
    Guid ReviewerId,
    string ReviewerName,
    Guid ContractId,
    Guid ReviewedUserId,
    string ReviewedUserName,
    DateTime CreatedAt
);
}
