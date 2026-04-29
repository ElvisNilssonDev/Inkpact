using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Gigs.Queries.SearchGigs
{
    public class SearchGigsValidator : AbstractValidator<SearchGigsQuery>
    {
        public SearchGigsValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50).WithMessage("PageSize must be between 1 and 50.");

            RuleFor(x => x.MaxBudget)
                .GreaterThan(0).When(x => x.MaxBudget.HasValue)
                .WithMessage("MaxBudget must be greater than 0.");
        }
    }
}
