using Application.Gigs.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.GetGigById
{
    public record GetGigByIdQuery(Guid GigId) : IRequest<OperationResult<GigDto>>;
}
