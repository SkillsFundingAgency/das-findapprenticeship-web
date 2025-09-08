using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class VolunteeringAndWorkExperienceSummaryViewModelValidator : AbstractValidator<VolunteeringAndWorkExperienceSummaryViewModel>
{
    public VolunteeringAndWorkExperienceSummaryViewModelValidator()
    {
        RuleFor(x => x.IsSectionCompleted)
            .NotNull()
            .WithMessage("Select if you have finished this section");
    }
}