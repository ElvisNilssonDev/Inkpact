using System;
using System.Collections.Generic;
using System.Text;
using Application.Auth.DTOs;
using Application.Common.Extensions;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, OperationResult<AuthResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IJwtService _jwt;

        public RegisterHandler(IUnitOfWork uow, IJwtService jwt)
        {
            _uow = uow;
            _jwt = jwt;
        }

        public async Task<OperationResult<AuthResponseDto>> Handle(
            RegisterCommand request,
            CancellationToken ct)
        {
            if (await _uow.Users.EmailExistsAsync(request.Email, ct))
                return "Email is already registered."
                    .AsFailure<AuthResponseDto>(OperationResultStatus.Conflict);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email.ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };

            await _uow.Users.AddAsync(user, ct);
            await _uow.SaveChangesAsync(ct);

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
