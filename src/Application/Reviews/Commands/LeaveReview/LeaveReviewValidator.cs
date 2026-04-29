using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Reviews.Commands.LeaveReview
{
    public class LeaveReviewValidator : AbstractValidator<LeaveReviewCommand>
    {
        public LeaveReviewValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.ReviewerId).NotEmpty();

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Comment is required.")
                .MinimumLength(10).WithMessage("Comment must be at least 10 characters.")
                .MaximumLength(1000).WithMessage("Comment cannot exceed 1000 characters.");
        }
    }
}
