using Application.Common.Extensions;
using Application.Contracts.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Queries.GetMyContracts
{
    public class GetMyContractsHandler : IRequestHandler<GetMyContractsQuery, OperationResult<IEnumerable<ContractDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetMyContractsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<ContractDto>>> Handle(
            GetMyContractsQuery request,
            CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user is null)
                return "User not found.".AsFailure<IEnumerable<ContractDto>>(OperationResultStatus.NotFound);

            var contracts = await _uow.Contracts.GetByUserIdAsync(request.UserId, ct);

            var dtos = new List<ContractDto>();
            foreach (var c in contracts)
            {
                var gig = await _uow.Gigs.GetByIdAsync(c.GigId, ct);
                var client = await _uow.Users.GetByIdAsync(c.ClientId, ct);
                var freelancer = await _uow.Users.GetByIdAsync(c.FreelancerId, ct);

                dtos.Add(new ContractDto(
                    c.Id,
                    c.AgreedRate,
                    c.StartDate,
                    c.EndDate,
                    c.Status,
                    c.GigId,
                    gig?.Title ?? "Unknown",
                    c.ClientId,
                    client?.Name ?? "Unknown",
                    c.FreelancerId,
                    freelancer?.Name ?? "Unknown",
                    c.CreatedAt
                ));
            }

            return dtos.AsEnumerable().AsSuccess();
        }
    }
}
