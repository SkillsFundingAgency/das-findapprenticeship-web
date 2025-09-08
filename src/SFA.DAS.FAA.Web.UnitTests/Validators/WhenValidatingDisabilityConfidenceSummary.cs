using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

[TestFixture]
public class WhenValidatingDisabilityConfidenceSummary
{
    private const string IsSectionCompleteErrorMessage = "Select if you have completed this section";

    [Test, MoqAutoData]
    public async Task And_Is_Section_Complete_Is_Null_Then_Model_Error(DisabilityConfidentSummaryViewModel model)
    {
        model.IsSectionCompleted = null;

        var sut = new DisabilityConfidentSummaryViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("IsSectionCompleted");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo(IsSectionCompleteErrorMessage);
        }
    }
}