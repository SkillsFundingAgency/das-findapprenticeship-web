using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class EqualityQuestionsEthnicSubGroupViewModelValidatorTest
{
    private const string NoEthnicSubGroupSelectionErrorMessage = "Select which of the following best describes your white background, or select 'Prefer not to say'";

    [TestCase(NoEthnicSubGroupSelectionErrorMessage, false, null)]
    [TestCase(NoEthnicSubGroupSelectionErrorMessage, false, "")]
    [TestCase(null, true, "British")]
    [TestCase(null, true, "Mixed")]
    public async Task Validate_EthnicSubGroup_EqualityGenderViewModel(string? errorMessage, bool isValid, string? group)
    {
        var model = new EqualityQuestionsEthnicSubGroupViewModel()
        {
            ApplicationId = Guid.NewGuid(),
            EthnicSubGroup = group
        };

        var sut = new EqualityEthnicSubGroupViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.EthnicSubGroup)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.EthnicSubGroup);
        }
    }
}
