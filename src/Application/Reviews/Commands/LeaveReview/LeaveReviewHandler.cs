using Application.Common.Extensions;
using Application.Reviews.DTOs;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reviews.Commands.LeaveReview
{
    public class LeaveReviewHandler : IRequestHandler<LeaveReviewCommand, OperationResult<ReviewDto>>
    {
        private readonly IUnitOfWork _uow;

        public LeaveReviewHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<OperationResult<ReviewDto>> Handle(
            LeaveReviewCommand request,
            CancellationToken ct)
        {
            var contract = await _uow.Contracts.GetByIdAsync(request.ContractId, ct);
            if (contract is null)
                return "Contract not found.".AsFailure<ReviewDto>(OperationResultStatus.NotFound);

            if (contract.ClientId != request.ReviewerId && contract.FreelancerId != request.ReviewerId)
                return "Only contract participants can leave a review."
                    .AsFailure<ReviewDto>(OperationResultStatus.Unauthorized);

            if (contract.Status != ContractStatus.Completed)
                return "Reviews can only be left on completed contracts."
                    .AsFailure<ReviewDto>(OperationResultStatus.Conflict);

            // Check if this user already reviewed this contract
            var existingReviews = await _uow.Reviews.GetByContractIdAsync(request.ContractId, ct);
            if (existingReviews.Any(r => r.ReviewerId == request.ReviewerId))
                return "You have already reviewed this contract."
                    .AsFailure<ReviewDto>(OperationResultStatus.Conflict);

            var review = new Review
            {
                ContractId = request.ContractId,
                ReviewerId = request.ReviewerId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var reviewedUserId = contract.ClientId == request.ReviewerId
                ? contract.FreelancerId
                : contract.ClientId;

            await _uow.Reviews.AddAsync(review, ct);
            await _uow.SaveChangesAsync(ct);

            var reviewer = await _uow.Users.GetByIdAsync(request.ReviewerId, ct);
            var reviewed = await _uow.Users.GetByIdAsync(reviewedUserId, ct);

            var dto = new ReviewDto(
                review.Id,
                review.Rating,
                review.Comment,
                review.ReviewerId,
                reviewer?.Name ?? "Unknown",
                review.ContractId,
                reviewedUserId,
                reviewed?.Name ?? "Unknown",
                review.CreatedAt
            );

            return dto.AsSuccess();
        }
    }
}
