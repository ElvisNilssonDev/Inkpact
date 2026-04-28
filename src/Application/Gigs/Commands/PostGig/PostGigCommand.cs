using Application.Gigs.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.PostGig
{
    public record PostGigCommand(
    string Title,
    string Description,
    decimal Budget,
    DateTime Deadline,
    string Tags,
    Guid ClientId
) : IRequest<OperationResult<GigDto>>;
}
