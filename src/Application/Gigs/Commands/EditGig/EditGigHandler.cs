using Application.Common.Extensions;
using Application.Gigs.DTOs;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.EditGig
{
    public class EditGigHandler : IRequestHandler<EditGigCommand, OperationResult<GigDto>>
    {
        private readonly IUnitOfWork _uow;

        public EditGigHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<GigDto>> Handle(
            EditGigCommand request,
            CancellationToken ct)
        {
            var gig = await _uow.Gigs.GetByIdAsync(request.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<GigDto>(OperationResultStatus.NotFound);

            if (gig.ClientId != request.CallerId)
                return "You are not authorized to edit this gig."
                    .AsFailure<GigDto>(OperationResultStatus.Unauthorized);

            if (gig.Status != GigStatus.Open)
                return "Only open gigs can be edited."
                    .AsFailure<GigDto>(OperationResultStatus.Conflict);

            gig.Title = request.Title;
            gig.Description = request.Description;
            gig.Budget = request.Budget;
            gig.Deadline = request.Deadline;
            gig.Tags = request.Tags;
            gig.UpdatedAt = DateTime.UtcNow;

            _uow.Gigs.Update(gig);
            await _uow.SaveChangesAsync(ct);

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
