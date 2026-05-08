using Application.Reviews.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reviews.Commands.LeaveReview
{
    public record LeaveReviewCommand(
    Guid ContractId,
    Guid ReviewerId,
    int Rating,
    string Comment
) : IRequest<OperationResult<ReviewDto>>;
}
