using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class VolunteeringAndWorkExperienceViewModelValidator : AbstractValidator<VolunteeringAndWorkExperienceViewModel>
{
    public VolunteeringAndWorkExperienceViewModelValidator()
    {
        RuleFor(x => x.DoYouWantToAddAnyVolunteeringOrWorkExperience)
        .NotNull()
        .WithMessage("Select if you want to add any volunteering or work experience");
    }
}