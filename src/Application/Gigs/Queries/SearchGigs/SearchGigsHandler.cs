using Application.Common.Pagination;
using Application.Gigs.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.SearchGigs
{
    public class SearchGigsHandler : IRequestHandler<SearchGigsQuery, OperationResult<PagedResult<GigDto>>>
    {
        private readonly IUnitOfWork _uow;

        public SearchGigsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<PagedResult<GigDto>>> Handle(
            SearchGigsQuery request,
            CancellationToken ct)
        {
            var allMatches = await _uow.Gigs.SearchAsync(request.Tag, request.MaxBudget, ct);
            var matches = allMatches.ToList();

            var paged = matches
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var dtos = new List<GigDto>();
            foreach (var gig in paged)
            {
                var client = await _uow.Users.GetByIdAsync(gig.ClientId, ct);
                dtos.Add(new GigDto(
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
                ));
            }

            var result = new PagedResult<GigDto>(
                dtos,
                request.Page,
                request.PageSize,
                matches.Count
            );

            return OperationResult<PagedResult<GigDto>>.Success(result);
        }
    }
}
