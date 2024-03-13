using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

[TestFixture]
public class WhenValidatingInterviewAdjustmentsSummary
{
    private const string IsSectionCompleteErrorMessage = "Select if you have completed this section";

    [Test, MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task And_Input_Is_Valid_Then_No_Model_Errors(
        bool doYouWantInterviewAdjustments,
        InterviewAdjustmentSummaryViewModel model)
    {
        model.IsSectionCompleted = doYouWantInterviewAdjustments;

        var sut = new InterviewAdjustmentsSummaryViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        result.IsValid.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task And_Is_Section_Complete_Is_Null_Then_Model_Error(InterviewAdjustmentSummaryViewModel model)
    {
        model.IsSectionCompleted = null;

        var sut = new InterviewAdjustmentsSummaryViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("IsSectionCompleted");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo(IsSectionCompleteErrorMessage);
        }
    }
}