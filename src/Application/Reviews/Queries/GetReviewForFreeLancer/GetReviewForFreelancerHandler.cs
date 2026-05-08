using Application.Common.Extensions;
using Application.Reviews.DTOs;
using Domain.Common;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reviews.Queries.GetReviewForFreeLancer
{
    public class GetReviewsForFreelancerHandler : IRequestHandler<GetReviewsForFreelancerQuery, OperationResult<IEnumerable<ReviewDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetReviewsForFreelancerHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<IEnumerable<ReviewDto>>> Handle(
            GetReviewsForFreelancerQuery request,
            CancellationToken ct)
        {
            var freelancer = await _uow.Users.GetByIdAsync(request.FreelancerId, ct);
            if (freelancer is null)
                return "Freelancer not found.".AsFailure<IEnumerable<ReviewDto>>(OperationResultStatus.NotFound);

            var reviews = await _uow.Reviews.GetByReviewedUserIdAsync(request.FreelancerId, ct);

            var dtos = new List<ReviewDto>();
            foreach (var r in reviews)
            {
                var reviewer = await _uow.Users.GetByIdAsync(r.ReviewerId, ct);
                dtos.Add(new ReviewDto(
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.ReviewerId,
                    reviewer?.Name ?? "Unknown",
                    r.ContractId,
                    request.FreelancerId,
                    freelancer.Name,
                    r.CreatedAt
                ));
            }

            return dtos.AsEnumerable().AsSuccess();
        }
    }
}
