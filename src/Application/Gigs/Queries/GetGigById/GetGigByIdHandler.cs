using Application.Common.Extensions;
using Application.Gigs.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.GetGigById
{
    public class GetGigByIdHandler : IRequestHandler<GetGigByIdQuery, OperationResult<GigDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetGigByIdHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<GigDto>> Handle(
            GetGigByIdQuery request,
            CancellationToken ct)
        {
            var gig = await _uow.Gigs.GetByIdAsync(request.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<GigDto>(OperationResultStatus.NotFound);

            var client = await _uow.Users.GetByIdAsync(gig.ClientId, ct);

            var dto = new GigDto(
                gig.Id,
                gig.Title,
                gig.Description,
                gig.Budget,
                gig.Deadline,
                gig.Status,
                gig.Tags,
                gig.ClientId,
                client?.Name ?? "Unknown",
                gig.CreatedAt
            );

            return dto.AsSuccess();
        }
    }
}
