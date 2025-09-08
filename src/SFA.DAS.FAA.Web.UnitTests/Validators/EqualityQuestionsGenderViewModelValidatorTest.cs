using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class EqualityQuestionsGenderViewModelValidatorTest
{
    private const string NoSexSelectionErrorMessage = "Select a sex or 'Prefer not to say'";
    private const string NoGenderSelectionErrorMessage = "Select if the gender you identify with is the same as your sex registered or birth, or select 'Prefer not to say'";

    [TestCase(NoSexSelectionErrorMessage, false, null, "Yes")]
    [TestCase(NoSexSelectionErrorMessage, false, "", "Yes")]
    [TestCase(null, true, "Male", "Yes")]
    [TestCase(null, true, "Female", "No")]
    public async Task Validate_Sex_EqualityGenderViewModel(string? errorMessage, bool isValid, string? sex, string? gender)
    {
        var model = new EqualityQuestionsGenderViewModel
        {
            ApplicationId = Guid.NewGuid(),
            Sex = sex,
            IsGenderIdentifySameSexAtBirth = gender
        };

        var sut = new EqualityQuestionsGenderViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.Sex)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Sex);
        }
    }

    [TestCase(NoGenderSelectionErrorMessage, false, "Female", null)]
    [TestCase(NoGenderSelectionErrorMessage, false, "Female", "")]
    [TestCase(null, true, "Male", "Yes")]
    [TestCase(null, true, "Female", "No")]
    public async Task Validate_Gender_EqualityGenderViewModel(string? errorMessage, bool isValid, string? sex, string? gender)
    {
        var model = new EqualityQuestionsGenderViewModel
        {
            ApplicationId = Guid.NewGuid(),
            Sex = sex,
            IsGenderIdentifySameSexAtBirth = gender
        };

        var sut = new EqualityQuestionsGenderViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.IsGenderIdentifySameSexAtBirth)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.IsGenderIdentifySameSexAtBirth);
        }
    }
}