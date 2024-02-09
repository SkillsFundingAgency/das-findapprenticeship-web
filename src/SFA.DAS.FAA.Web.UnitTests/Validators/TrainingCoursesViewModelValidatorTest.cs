using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class TrainingCoursesViewModelValidatorTest
{
    private const string NoSelectionErrorMessage = "Select if you want to add any training courses";

    [TestCase(NoSelectionErrorMessage, false, null)]
    [TestCase(null, true, true)]
    [TestCase(null, true, false)]
    public async Task Validate_TrainingCourses(string? errorMessage, bool isValid, bool? doYouWantToAddAnyTrainingCourses)
    {
        var model = new TrainingCoursesViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            BackLinkUrl = "",
            DoYouWantToAddAnyTrainingCourses = doYouWantToAddAnyTrainingCourses
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
}
