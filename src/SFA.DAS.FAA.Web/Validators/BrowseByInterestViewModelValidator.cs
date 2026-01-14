using FluentValidation;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Validators;

public class BrowseByInterestViewModelValidator : AbstractValidator<BrowseByInterestViewModel>
{
    public BrowseByInterestViewModelValidator()
    {
        RuleFor(x => x.SelectedRouteIds)
            .NotEmpty()
            .WithMessage("Select at least one job category");
    }
}