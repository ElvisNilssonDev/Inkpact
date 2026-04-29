using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.PostGig
{
    public class PostGigValidator : AbstractValidator<PostGigCommand>
    {
        public PostGigValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(20).WithMessage("Description must be at least 20 characters.")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");

            RuleFor(x => x.Budget)
                .GreaterThan(0).WithMessage("Budget must be greater than 0.");

            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future.");

            RuleFor(x => x.Tags)
                .NotEmpty().WithMessage("At least one tag is required.")
                .MaximumLength(200);

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("ClientId is required.");
        }
    }
}
