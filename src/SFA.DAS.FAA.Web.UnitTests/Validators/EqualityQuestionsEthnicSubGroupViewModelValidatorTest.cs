using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class EqualityQuestionsEthnicSubGroupViewModelValidatorTest
{
    private const string NoEthnicWhiteSubGroupSelectionErrorMessage = "Select which of the following best describes your white background, or select 'Prefer not to say'";
    private const string NoEthnicMixedSubGroupSelectionErrorMessage = "Select which of the following best describes your mixed or multiple ethnic groups background, or select 'Prefer not to say'";

    [TestCase(NoEthnicWhiteSubGroupSelectionErrorMessage, false, null)]
    [TestCase(NoEthnicWhiteSubGroupSelectionErrorMessage, false, "")]
    [TestCase(null, true, "British")]
    [TestCase(null, true, "Mixed")]
    public async Task Validate_EthnicSubGroup_White_EqualityGenderViewModel(string? errorMessage, bool isValid, string? group)
    {
        var model = new EqualityQuestionsEthnicSubGroupWhiteViewModel
        {
            ApplicationId = Guid.NewGuid(),
            EthnicSubGroup = group
        };

        var sut = new EqualityEthnicSubGroupWhiteViewModelValidator();
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

    [TestCase(NoEthnicMixedSubGroupSelectionErrorMessage, false, null)]
    [TestCase(NoEthnicMixedSubGroupSelectionErrorMessage, false, "")]
    [TestCase(null, true, "White")]
    [TestCase(null, true, "Asian")]
    public async Task Validate_EthnicSubGroup_Mixed_EqualityGenderViewModel(string? errorMessage, bool isValid, string? group)
    {
        var model = new EqualityQuestionsEthnicSubGroupMixedViewModel
        {
            ApplicationId = Guid.NewGuid(),
            EthnicSubGroup = group
        };

        var sut = new EqualityEthnicSubGroupMixedViewModelValidator();
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
