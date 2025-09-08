using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingCandidatePostcodeAddress
{
    private readonly string IllegalInput = "Enter a full UK postcode";
    private readonly string PostcodeRequired = "Enter a postcode to search for your address or select 'Enter my address manually'";

    [Test, MoqInlineAutoData("abcdefghij123")]
    public async Task And_Input_Is_Too_Long_Then_Validation_Error(string postcode, PostcodeAddressViewModel model)
    {
        model.Postcode = postcode;
        var sut = new PostcodeAddressViewModelValidator();

        var result = await sut.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Postcode).WithErrorMessage(IllegalInput);
    }

    [Test, MoqInlineAutoData("abc 12*")]
    public async Task And_Input_Has_Special_Characters_Then_Validation_Error(string postcode, PostcodeAddressViewModel model)
    {
        model.Postcode = postcode;
        var sut = new PostcodeAddressViewModelValidator();

        var result = await sut.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Postcode).WithErrorMessage(IllegalInput);
    }

    [Test, MoqAutoData]
    public async Task And_Input_Is_Null_Then_Validation_Error(PostcodeAddressViewModel model)
    {
        model.Postcode = null;
        var sut = new PostcodeAddressViewModelValidator();

        var result = await sut.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Postcode).WithErrorMessage(PostcodeRequired);
    }

    [Test, MoqInlineAutoData("CV12 0GE")]
    public async Task And_Input_Is_Valid_Then_No_Validation_Errors(string postcode, PostcodeAddressViewModel model)
    {
        model.Postcode = postcode;
        var sut = new PostcodeAddressViewModelValidator();

        var result = await sut.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Postcode);
    }
}