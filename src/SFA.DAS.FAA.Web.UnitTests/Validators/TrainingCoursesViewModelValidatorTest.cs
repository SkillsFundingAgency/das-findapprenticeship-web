using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class TrainingCoursesViewModelValidatorTest
{
    private const string DoYouWantToAddAnyTrainingCoursesErrorMessage = "Select if you want to add any training courses";
    private const string IsSectionCompleteErrorMessage = "Select if you have finished this section";

    [TestCase(DoYouWantToAddAnyTrainingCoursesErrorMessage, false, null, null, false)]
    [TestCase(null, true, true, null, false)]
    [TestCase(null, true, false, null, false)]
    public async Task AndShowTrainingCoursesIsFalse_Validate_TrainingCourses(
        string? errorMessage,
        bool isValid,
        bool? doYouWantToAddAnyTrainingCourses,
        bool? isSectionComplete,
        bool? showTrainingCourses)
    {
        var model = new TrainingCoursesViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            BackLinkUrl = "",
            DoYouWantToAddAnyTrainingCourses = doYouWantToAddAnyTrainingCourses,
            IsSectionComplete = isSectionComplete,
            ShowTrainingCoursesAchieved = (bool)showTrainingCourses
        };

        var sut = new TrainingCoursesViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.DoYouWantToAddAnyTrainingCourses)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.DoYouWantToAddAnyTrainingCourses);
        }
    }

    [TestCase(IsSectionCompleteErrorMessage, false, true, null, true)]
    [TestCase(IsSectionCompleteErrorMessage, false, false, null, true)]
    [TestCase(IsSectionCompleteErrorMessage, false, null, null, true)]
    [TestCase(null, true, null, true, true)]
    [TestCase(null, true, null, false, true)]
    public async Task AndShowTrainingCoursesIsTrue_Validate_TrainingCourses(
    string? errorMessage,
    bool isValid,
    bool? doYouWantToAddAnyTrainingCourses,
    bool? isSectionComplete,
    bool? showTrainingCourses)
    {
        var model = new TrainingCoursesViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            BackLinkUrl = "",
            DoYouWantToAddAnyTrainingCourses = doYouWantToAddAnyTrainingCourses,
            IsSectionComplete = isSectionComplete,
            ShowTrainingCoursesAchieved = (bool)showTrainingCourses
        };

        var sut = new TrainingCoursesViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.IsSectionComplete)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.IsSectionComplete);
        }
    }
}
