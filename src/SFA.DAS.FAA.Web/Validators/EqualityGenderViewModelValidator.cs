using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityGenderViewModelValidator : AbstractValidator<EqualityGenderViewModel>
{
    private const string NoSexSelectionErrorMessage = "Select a sex or 'Prefer not to say'";
    private const string NoGenderSelectionErrorMessage = "Select if the gender you identify with is the same as your sex registered or birth, or select 'Prefer not to say'";

    public EqualityGenderViewModelValidator()
    {
        RuleFor(x => x.Sex).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoSexSelectionErrorMessage)
            .NotEmpty().WithMessage(NoSexSelectionErrorMessage);

        RuleFor(x => x.Gender).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoGenderSelectionErrorMessage)
            .NotEmpty().WithMessage(NoGenderSelectionErrorMessage);
    }
}
