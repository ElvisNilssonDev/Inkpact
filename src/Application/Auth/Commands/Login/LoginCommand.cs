using Application.Auth.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<OperationResult<AuthResponseDto>>;

}
