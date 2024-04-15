using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class EqualityQuestionsEthnicSubGroupViewModelValidatorTest
{
    private const string NoEthnicWhiteSubGroupSelectionErrorMessage = "Select which of the following best describes your white background, or select 'Prefer not to say'";
    private const string NoEthnicMixedSubGroupSelectionErrorMessage = "Select which of the following best describes your mixed or multiple ethnic groups background, or select 'Prefer not to say'";
    private const string NoEthnicAsianSubGroupSelectionErrorMessage = "Select which of the following best describes your Asian or Asian British background, or select 'Prefer not to say'";
    private const string NoEthnicBlackSubGroupSelectionErrorMessage = "Select which of the following best describes your Black, African, Caribbean or Black British background, or select 'Prefer not to say'";

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

    [TestCase(NoEthnicAsianSubGroupSelectionErrorMessage, false, null)]
    [TestCase(NoEthnicAsianSubGroupSelectionErrorMessage, false, "")]
    [TestCase(null, true, "Indian")]
    [TestCase(null, true, "Pakistani")]
    [TestCase(null, true, "Bangladeshi ")]
    [TestCase(null, true, "Chinese")]
    [TestCase(null, true, "Any other Asian background")]
    public async Task Validate_EthnicSubGroup_Asian_EqualityGenderViewModel(string? errorMessage, bool isValid, string? group)
    {
        var model = new EqualityQuestionsEthnicSubGroupAsianViewModel
        {
            ApplicationId = Guid.NewGuid(),
            EthnicSubGroup = group
        };

        var sut = new EqualityEthnicSubGroupAsianViewModelValidator();
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

    [TestCase(NoEthnicBlackSubGroupSelectionErrorMessage, false, null)]
    [TestCase(NoEthnicBlackSubGroupSelectionErrorMessage, false, "")]
    [TestCase(null, true, "African")]
    [TestCase(null, true, "Caribbean")]
    [TestCase(null, true, "Any other Black, African or Caribbean background")]
    public async Task Validate_EthnicSubGroup_Black_EqualityGenderViewModel(string? errorMessage, bool isValid, string? group)
    {
        var model = new EqualityQuestionsEthnicSubGroupBlackViewModel
        {
            ApplicationId = Guid.NewGuid(),
            EthnicSubGroup = group
        };

        var sut = new EqualityEthnicSubGroupBlackViewModelValidator();
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
