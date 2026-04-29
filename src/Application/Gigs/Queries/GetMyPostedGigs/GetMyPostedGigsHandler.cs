using Application.Common.Extensions;
using Application.Gigs.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.GetMyPostedGigs
{
    public class GetMyPostedGigsHandler : IRequestHandler<GetMyPostedGigsQuery, OperationResult<IEnumerable<GigDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetMyPostedGigsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<GigDto>>> Handle(
            GetMyPostedGigsQuery request,
            CancellationToken ct)
        {
            var client = await _uow.Users.GetByIdAsync(request.ClientId, ct);
            if (client is null)
                return "Client not found.".AsFailure<IEnumerable<GigDto>>(OperationResultStatus.NotFound);

            var gigs = await _uow.Gigs.GetByClientIdAsync(request.ClientId, ct);

            var dtos = gigs.Select(gig => new GigDto(
                gig.Id,
                gig.Title,
                gig.Description,
                gig.Budget,
                gig.Deadline,
                gig.Status,
                gig.Tags,
                gig.ClientId,
                client.Name,
                gig.CreatedAt
            ));

            return dtos.AsSuccess();
        }
    }
}
