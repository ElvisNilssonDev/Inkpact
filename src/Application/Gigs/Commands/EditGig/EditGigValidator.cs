using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Commands.EditGig
{
    public class EditGigValidator : AbstractValidator<EditGigCommand>
    {
        public EditGigValidator()
        {
            RuleFor(x => x.GigId)
                .NotEmpty().WithMessage("GigId is required.");

            RuleFor(x => x.CallerId)
                .NotEmpty().WithMessage("CallerId is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(20)
                .MaximumLength(2000);

            RuleFor(x => x.Budget)
                .GreaterThan(0).WithMessage("Budget must be greater than 0.");

            RuleFor(x => x.Deadline)
                .GreaterThan(DateTime.UtcNow).WithMessage("Deadline must be in the future.");

            RuleFor(x => x.Tags)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
