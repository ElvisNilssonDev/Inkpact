using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.CloseGig
{
    public record CloseGigCommand(Guid GigId, Guid CallerId) : IRequest<OperationResult<bool>>;
}
