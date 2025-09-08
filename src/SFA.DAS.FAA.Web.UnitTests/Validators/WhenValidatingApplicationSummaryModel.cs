using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

[TestFixture]
public class WhenValidatingApplicationSummaryModel
{
    private const string IsSectionCompleteErrorMessage = "Select that you understand you won’t be able to make changes after submitting your application";

    [Test, MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task And_Input_Is_Valid_Then_No_Model_Errors(
        bool isConsentProvided,
        ApplicationSummaryViewModel model)
    {
        model.IsConsentProvided = isConsentProvided;

        var sut = new ApplicationSummaryViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        if (isConsentProvided)
        {
            result.IsValid.Should().BeTrue();
        }
        else
        {
            using (new AssertionScope())
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor("IsConsentProvided");
                result.Errors[0].ErrorMessage.Should().BeEquivalentTo(IsSectionCompleteErrorMessage);
            }
        }
    }
}