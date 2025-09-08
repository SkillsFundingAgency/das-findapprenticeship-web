using FluentValidation.TestHelper;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

public class WhenValidatingPhoneNumberViewModel
{
    [Test, MoqInlineAutoData("07806 172382")]
    [MoqInlineAutoData("01632 960 001")]
    [MoqInlineAutoData("+44 01632 960 001")]
    [MoqInlineAutoData("+447222555555")]
    [MoqInlineAutoData("+447222555555")]
    [MoqInlineAutoData("+44 7222 555 555")]
    [MoqInlineAutoData("+44 7222 555555 #2222")]
    public async Task And_PhoneNumber_Is_Valid_Then_Validation_Success(string phoneNumber)
    {
        var model = new PhoneNumberViewModel
        {
            PhoneNumber = phoneNumber
        };

        var validator = new PhoneNumberViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().Be(true);
    }

    [Test, MoqInlineAutoData("abc")]
    [MoqInlineAutoData("+44 089 44321")]
    [MoqInlineAutoData("+44 089 44")]
    [MoqInlineAutoData("073063345908")]
    [MoqInlineAutoData("073063")]
    public async Task And_PhoneNumber_Is_Invalid_Then_Model_Error(string phoneNumber)
    {
        var model = new PhoneNumberViewModel
        {
            PhoneNumber = phoneNumber
        };

        var validator = new PhoneNumberViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Enter a telephone number, like 01632 960 001, 07700 900 982 or +44 808 157 0192");
    }

    [Test, MoqAutoData]
    public async Task And_PhoneNumber_Is_Null_Then_Model_Error(PhoneNumberViewModel model)
    {
        model.PhoneNumber = null;
        var validator = new PhoneNumberViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber).WithErrorMessage("Enter your telephone number");
    }
}