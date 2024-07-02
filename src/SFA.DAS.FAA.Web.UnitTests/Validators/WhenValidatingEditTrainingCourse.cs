using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class WhenValidatingEditTrainingCourse
{
    private const string CourseNameErrorMessage = "Enter the name of this training course";
    private const string YearAchievedErrorMessage = "Enter the year you finished this training course";
    private const string YearAchievedInvalidFormatErrorMessage = "Enter a real year";
    private const string YearAchievedIsInTheFutureErrorMessage = "The year you finished this training course must be in the past";

    [Test, MoqInlineAutoData(null, "2014", CourseNameErrorMessage, false)]
    [MoqInlineAutoData("", "2014", CourseNameErrorMessage, false)]
    public async Task AndCourseNameInvalid_ValidationResultIsInvalid(string? courseName, string? yearAchieved, string? errorMessage, bool isValid, Mock<IDateTimeService> dateTimeService)
    {
        var model = new EditTrainingCourseViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            CourseName = courseName,
            YearAchieved = yearAchieved
        };

        var sut = new EditTrainingCourseViewModelValidator(dateTimeService.Object);
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.CourseName)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.CourseName);
        }
    }

    [Test, MoqInlineAutoData("course name", null, YearAchievedErrorMessage, false)]
    [MoqInlineAutoData("course name", "abc", YearAchievedInvalidFormatErrorMessage, false)]
    [MoqInlineAutoData("course name", "123", YearAchievedInvalidFormatErrorMessage, false)]
    [MoqInlineAutoData("course name", "20345", YearAchievedInvalidFormatErrorMessage, false)]
    [MoqInlineAutoData("course name", "2035", YearAchievedIsInTheFutureErrorMessage, false)]
    public async Task AndYearAchievedIsInvalid_ValidationResultIsInvalid(string? courseName, string? yearAchieved, string? errorMessage, bool isValid, Mock<IDateTimeService> dateTimeService)
    {
        var model = new EditTrainingCourseViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            CourseName = courseName,
            YearAchieved = yearAchieved
        };

        var sut = new EditTrainingCourseViewModelValidator(dateTimeService.Object);
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.YearAchieved)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.YearAchieved);
        }
    }

    [Test, MoqInlineAutoData("course name", "2022", null, true)]
    public async Task AndInputIsValid_ValidationResultIsValid(string? courseName, string? yearAchieved, string? errorMessage, bool isValid, Mock<IDateTimeService> dateTimeService)
    {
        var model = new EditTrainingCourseViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            CourseName = courseName,
            YearAchieved = yearAchieved
        };

        var sut = new EditTrainingCourseViewModelValidator(dateTimeService.Object);
        var result = await sut.TestValidateAsync(model);

        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Test, MoqAutoData]
    public async Task And_Input_Has_Invalid_Characters_Then_Invalid(
        Mock<IDateTimeService> dateTimeService)
    {
        var model = new EditTrainingCourseViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            CourseName = "<script>alert()</script>",
            YearAchieved = "2022"
        };

        var sut = new EditTrainingCourseViewModelValidator(dateTimeService.Object);
        var result = await sut.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(c=>c.CourseName);
    }
}
