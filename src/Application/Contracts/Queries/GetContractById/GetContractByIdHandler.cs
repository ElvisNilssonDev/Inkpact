using Application.Common.Extensions;
using Application.Contracts.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Queries.GetContractById
{
    public class GetContractByIdHandler : IRequestHandler<GetContractByIdQuery, OperationResult<ContractDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetContractByIdHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<ContractDto>> Handle(
            GetContractByIdQuery request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<ContractDto>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.CallerId && contract.FreelancerId != request.CallerId)
                return "You are not authorized to view this contract."
                    .AsFailure<ContractDto>(OperationResultStatus.Unauthorized);

            var gig = await _uow.Gigs.GetByIdAsync(contract.GigId, ct);
            var client = await _uow.Users.GetByIdAsync(contract.ClientId, ct);
            var freelancer = await _uow.Users.GetByIdAsync(contract.FreelancerId, ct);

            var dto = new ContractDto(
                contract.Id,
                contract.AgreedRate,
                contract.StartDate,
                contract.EndDate,
                contract.Status,
                contract.TerminationReason,
                contract.TerminatedAt,
                contract.GigId,
                gig?.Title ?? "Unknown",
                contract.ClientId,
                client?.Name ?? "Unknown",
                contract.FreelancerId,
                freelancer?.Name ?? "Unknown",
                contract.CreatedAt
            );

            return dto.AsSuccess();
        }
    }
}
