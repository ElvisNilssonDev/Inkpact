using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Milestones.Commands.AddMilestone
{
    public class AddMilestoneValidator : AbstractValidator<AddMilestoneCommand>
    {
        public AddMilestoneValidator()
        {
            RuleFor(x => x.ContractId).NotEmpty();
            RuleFor(x => x.CallerId).NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000);

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
        }
    }
}
