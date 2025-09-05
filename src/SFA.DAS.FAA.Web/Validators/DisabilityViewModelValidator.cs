using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class DisabilityViewModelValidator : AbstractValidator<DisabilityConfidentViewModel>
{
    public DisabilityViewModelValidator()
    {
        RuleFor(x => x.ApplyUnderDisabilityConfidentScheme)
            .NotNull()
            .WithMessage("Select if you want to apply under the Disability Confident scheme");
    }
}