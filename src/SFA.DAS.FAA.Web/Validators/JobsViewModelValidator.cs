using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class JobsViewModelValidator : AbstractValidator<JobsViewModel>
{
    public JobsViewModelValidator()
    {
        RuleFor(x => x.DoYouWantToAddAnyJobs)
            .NotNull()
            .When(x => !x.ShowJobHistory)
            .WithMessage("Select if you want to add any jobs");

        RuleFor(x => x.IsSectionCompleted)
            .NotNull()
            .When(x => x.ShowJobHistory)
            .WithMessage("Select if you have finished this section");
    }
}