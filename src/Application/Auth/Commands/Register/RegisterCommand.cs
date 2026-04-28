using Application.Auth.DTOs;
using Domain.Common;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.Commands.Register
{
    public record RegisterCommand(
        string Name,
        string Email,
        string Password,
        UserRole Role
    ) : IRequest<OperationResult<AuthResponseDto>>;
}
