using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.DTOs
{
    public record AuthResponseDto(
        Guid UserId,
        string Name,
        string Email,
        UserRole Role,
        string Token,
        DateTime ExpiresAt
    );
}
