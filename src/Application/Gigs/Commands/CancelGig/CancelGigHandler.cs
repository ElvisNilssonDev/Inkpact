using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.CancelGig
{
    public class CancelGigHandler : IRequestHandler<CancelGigCommand, OperationResult<bool>>
    {
        private readonly IUnitOfWork _uow;

        public CancelGigHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<bool>> Handle(
            CancelGigCommand request,
            CancellationToken ct)
        {
            var gig = await _uow.Gigs.GetByIdAsync(request.GigId, ct);
            if (gig is null)
                return "Gig not found.".AsFailure<bool>(OperationResultStatus.NotFound);

            if (gig.ClientId != request.CallerId)
                return "You are not authorized to cancel this gig."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (gig.Status == GigStatus.Cancelled)
                return "Gig is already cancelled."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            if (gig.Status == GigStatus.Completed)
                return "Completed gigs cannot be cancelled."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            gig.Status = GigStatus.Cancelled;
            gig.UpdatedAt = DateTime.UtcNow;

            _uow.Gigs.Update(gig);
            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
