using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class EqualityQuestionsEthnicGroupViewModelValidatorTest
{
    private const string NoEthnicGroupSelectionErrorMessage = "Select an ethnic group or 'Prefer not to say'";

    [TestCase(NoEthnicGroupSelectionErrorMessage, false, null)]
    [TestCase(NoEthnicGroupSelectionErrorMessage, false, "")]
    [TestCase(null, true, "British")]
    [TestCase(null, true, "Mixed")]
    public async Task Validate_EthnicGroup_EqualityGenderViewModel(string? errorMessage, bool isValid, string? group)
    {
        var model = new EqualityQuestionsEthnicGroupViewModel
        {
            ApplicationId = Guid.NewGuid(),
            EthnicGroup = group
        };

        var sut = new EqualityEthnicGroupViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.EthnicGroup)
                .WithErrorMessage(errorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.EthnicGroup);
        }
    }
}
