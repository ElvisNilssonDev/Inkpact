using Application.Gigs.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.GetMyPostedGigs
{
    public record GetMyPostedGigsQuery(Guid ClientId) : IRequest<OperationResult<IEnumerable<GigDto>>>;


}
