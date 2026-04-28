using Application.Gigs.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.EditGig
{
    public record EditGigCommand(
        Guid GigId,
        Guid CallerId,
        string Title,
        string Description,
        decimal Budget,
        DateTime Deadline,
        string Tags) : IRequest<OperationResult<GigDto>>;
}
