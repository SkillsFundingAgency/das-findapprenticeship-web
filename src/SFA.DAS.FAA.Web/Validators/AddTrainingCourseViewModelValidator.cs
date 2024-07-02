using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.FAA.Web.Validators;

public class AddTrainingCourseViewModelValidator : AbstractValidator<AddTrainingCourseViewModel>
{
    public AddTrainingCourseViewModelValidator(IDateTimeService dateTimeService)
    {
        Include(new TrainingCourseViewModelBaseValidator(dateTimeService));
    }
}

public class EditTrainingCourseViewModelValidator : AbstractValidator<EditTrainingCourseViewModel>
{
    public EditTrainingCourseViewModelValidator(IDateTimeService dateTimeService)
    {
        Include(new TrainingCourseViewModelBaseValidator(dateTimeService));
    }
}

public class TrainingCourseViewModelBaseValidator : AbstractValidator<TrainingCourseViewModelBase>
{
    private const string CourseNameErrorMessage = "Enter the name of this training course";
    private const string YearAchievedErrorMessage = "Enter the year you finished this training course";
    private const string YearAchievedInvalidFormatErrorMessage = "Enter a real year";
    private const string YearAchievedIsInTheFutureErrorMessage = "The year you finished this training course must be in the past";

    public TrainingCourseViewModelBaseValidator(IDateTimeService dateTimeService)
    {
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage(CourseNameErrorMessage);

        RuleFor(x => x.CourseName)
            .ValidFreeTextCharacters();

        RuleFor(x => x.YearAchieved)
            .NotEmpty().WithMessage(YearAchievedErrorMessage)
            .NotNull().WithMessage(YearAchievedErrorMessage)
            .Must(x => x != null && int.TryParse(x, out _)).WithMessage(YearAchievedInvalidFormatErrorMessage)
            .Must(x => x != null && x.Length is 4).WithMessage(YearAchievedInvalidFormatErrorMessage)
            .Must(x => x != null && int.TryParse(x, out _) && int.Parse(x) <= dateTimeService.GetDateTime().Year).WithMessage(YearAchievedIsInTheFutureErrorMessage);
    }
}
