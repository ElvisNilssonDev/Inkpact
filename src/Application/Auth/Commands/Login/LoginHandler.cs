using Application.Auth.DTOs;
using Application.Common.Extensions;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Auth.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, OperationResult<AuthResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwt;

        public LoginHandler(IUnitOfWork uow, IJwtService jwt)
        {
            _uow = uow;
            _jwt = jwt;
        }

        public async Task<OperationResult<AuthResponseDto>> Handle(
            LoginCommand request,
            CancellationToken ct)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Email.ToLowerInvariant(), ct);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return "Invalid email or password."
                    .AsFailure<AuthResponseDto>(OperationResultStatus.Unauthorized);

            var token = _jwt.GenerateToken(user);
            var response = new AuthResponseDto(
                user.Id,
                user.Name,
                user.Email,
                user.Role,
                token,
                DateTime.UtcNow.AddMinutes(45)
            );

            return response.AsSuccess();
        }
    }
}
