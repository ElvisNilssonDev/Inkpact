using Application.Reviews.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reviews.Queries.GetReviewForFreeLancer
{
    public record GetReviewsForFreelancerQuery(Guid FreelancerId) : IRequest<OperationResult<IEnumerable<ReviewDto>>>;
}
