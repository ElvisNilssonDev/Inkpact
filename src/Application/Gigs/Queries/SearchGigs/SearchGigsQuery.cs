using Application.Common.Pagination;
using Application.Gigs.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.SearchGigs
{
    public record SearchGigsQuery(
    string? Tag,
    decimal? MaxBudget,
    int Page = 1,
    int PageSize = 20
) : IRequest<OperationResult<PagedResult<GigDto>>>;
}
