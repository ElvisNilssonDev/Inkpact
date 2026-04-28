using Application.Common.Extensions;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.CloseGig
{
    public class CloseGigHandler : IRequestHandler<CloseGigCommand, OperationResult<bool>>
    {
        private IUnitOfWork _uow;

        public CloseGigHandler(IUnitOfWork _uow) => _uow = _uow;

        public async Task<OperationResult<bool>> Handle(
            CloseGigCommand request,
            CancellationToken ct)
        {
            var gig = await _uow.Gigs.GetByIdAsync(request.GigId, ct);
            if (gig is null)
                return "Gig is not found".AsFailure<bool>(OperationResultStatus.NotFound);

            if (gig.ClientId != request.CallerId)
                return "You are not authorized to close this gig."
                    .AsFailure<bool>(OperationResultStatus.Unauthorized);

            if (gig.Status != GigStatus.Open)
                return "Only open deals can be closed."
                    .AsFailure<bool>(OperationResultStatus.Conflict);

            gig.Status = GigStatus.Completed;
            gig.UpdatedAt = DateTime.UtcNow;

            _uow.Gigs.Update(gig);
            await _uow.SaveChangesAsync(ct);

            return true.AsSuccess();
        }
    }
}
