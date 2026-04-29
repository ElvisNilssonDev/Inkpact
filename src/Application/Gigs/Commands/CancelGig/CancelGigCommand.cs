using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.CancelGig
{
    public record CancelGigCommand(Guid GigId, Guid CallerId) : IRequest<OperationResult<bool>>;
}
