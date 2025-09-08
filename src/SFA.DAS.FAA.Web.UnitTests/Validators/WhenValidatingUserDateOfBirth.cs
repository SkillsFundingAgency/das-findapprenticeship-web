using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingUserDateOfBirth
{
    private const string UserIsTooYoung = "You must be older than 13 to use Find an apprenticeship";
    private const string DateIsInFuture = "Date of birth must be in the past";
    private const string EmptyInput = "Enter your date of birth";

    [Test]
    public async Task And_User_Is_Too_Young_Then_Error()
    {
        var day = 30;
        var month = 10;
        var year = DateTime.Today.AddYears(-5).Year;

        var model = new DateOfBirthViewModel
        {
            DateOfBirth = new DayMonthYearDate(new DateTime(year, month, day))
        };

        var validator = new DateOfBirthViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth).WithErrorMessage(UserIsTooYoung);
    }

    [Test]
    public async Task And_DateTime_Is_In_The_Future_Then_Error()
    {
        var day = 30;
        var month = 10;
        var year = DateTime.Today.AddYears(5).Year;

        var model = new DateOfBirthViewModel
        {
            DateOfBirth = new DayMonthYearDate(new DateTime(year, month, day))
        };

        var validator = new DateOfBirthViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth).WithErrorMessage(DateIsInFuture);
    }

    [Test]
    public async Task And_User_Submits_Empty_Input_Then_Error()
    {
        var model = new DateOfBirthViewModel
        {
            DateOfBirth = null
        };

        var validator = new DateOfBirthViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(x => x.DateOfBirth).WithErrorMessage(EmptyInput);
    }
}