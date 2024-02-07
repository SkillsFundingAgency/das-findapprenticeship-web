using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class TrainingCoursesViewModelValidator : AbstractValidator<TrainingCoursesViewModel>
{
    public TrainingCoursesViewModelValidator()
    {
        RuleFor(x => x.DoYouWantToAddAnyTrainingCourses)
        .NotNull()
        .WithMessage("Add any professional training courses you want to include.");
    }
}
