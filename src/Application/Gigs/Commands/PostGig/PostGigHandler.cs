using Application.Common.Extensions;
using Application.Gigs.DTOs;
using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.PostGig
{
    public class PostGigHandler : IRequestHandler<PostGigCommand, OperationResult<GigDto>>
    {
        private readonly IUnitOfWork _uow;

        public PostGigHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<GigDto>> Handle(
            PostGigCommand request,
            CancellationToken ct)
        {
            var client = await _uow.Users.GetByIdAsync(request.ClientId, ct);
            if (client is null)
                return "Client not found.".AsFailure<GigDto>(OperationResultStatus.NotFound);

            var gig = new Gig
            {
                Title = request.Title,
                Description = request.Description,
                Budget = request.Budget,
                Deadline = request.Deadline,
                Tags = request.Tags,
                ClientId = request.ClientId
            };

            await _uow.Gigs.AddAsync(gig, ct);
            await _uow.SaveChangesAsync(ct);

            var dto = new GigDto(
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
            );

            return dto.AsSuccess();
        }
    }
}
