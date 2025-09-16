using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class TrainingCoursesViewModelValidator : AbstractValidator<TrainingCoursesViewModel>
{
    public TrainingCoursesViewModelValidator()
    {
        RuleFor(x => x.DoYouWantToAddAnyTrainingCourses)
            .NotNull()
            .When(x => x.ShowTrainingCoursesAchieved is false)
            .WithMessage("Select if you want to add any training courses");

        RuleFor(x => x.IsSectionComplete)
            .NotNull()
            .When(x => x.ShowTrainingCoursesAchieved)
            .WithMessage("Select if you have finished this section");
    }
}