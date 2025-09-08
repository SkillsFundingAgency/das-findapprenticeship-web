using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingInterviewAdjustments
{
    private readonly string IsSectionCompleteErrorMessage = "Select if you want to request interview support";
    private readonly string InterviewAdjustmentsDescriptionErrorMessage = "Enter your interview support request";

    [Test, MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task And_Input_Is_Valid_Then_No_Model_Errors(
    bool doYouWantInterviewAdjustments,
    InterviewAdjustmentsViewModel model)
    {
        model.DoYouWantInterviewAdjustments = doYouWantInterviewAdjustments;

        var sut = new InterviewAdjustmentsViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        result.IsValid.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task And_Is_Section_Complete_Is_Null_Then_Model_Error(InterviewAdjustmentsViewModel model)
    {
        model.DoYouWantInterviewAdjustments = null;

        var sut = new InterviewAdjustmentsViewModelValidator();
        var result = await sut.TestValidateAsync(model);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("DoYouWantInterviewAdjustments");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo(IsSectionCompleteErrorMessage);
        }
    }

    [Test, MoqAutoData]
    public async Task And_Is_Section_Complete_Is_True_But_User_Has_Not_Entered_Any_Input_Then_Model_Error(InterviewAdjustmentsViewModel viewModel)
    {
        viewModel.DoYouWantInterviewAdjustments = true;
        viewModel.InterviewAdjustmentsDescription = null;

        var sut = new InterviewAdjustmentsViewModelValidator();
        var result = await sut.TestValidateAsync(viewModel);

        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.ShouldHaveValidationErrorFor("InterviewAdjustmentsDescription");
            result.Errors[0].ErrorMessage.Should().BeEquivalentTo(InterviewAdjustmentsDescriptionErrorMessage);
        }
    }
    
    [Test, MoqAutoData]
    public async Task And_Input_Has_Invalid_Characters_Then_Invalid(InterviewAdjustmentsViewModel viewModel)
    {
        viewModel.DoYouWantInterviewAdjustments = true;
        viewModel.InterviewAdjustmentsDescription = "<script>alert()</script>";

        var sut = new InterviewAdjustmentsViewModelValidator();
        var result = await sut.TestValidateAsync(viewModel);

        result.ShouldHaveValidationErrorFor(c=>c.InterviewAdjustmentsDescription);
    }
}
