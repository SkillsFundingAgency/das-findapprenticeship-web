using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class WhenValidatingPhoneNumberViewModel
{
    [Test, MoqInlineAutoData("abc")]
    [MoqInlineAutoData("+44 089 44321")]
    [MoqInlineAutoData("+44 089 44")]
    [MoqInlineAutoData("073063345908")]
    [MoqInlineAutoData("073063")]
    public async Task And_PhoneNumber_Is_Invalid_Then_Model_Error(string phoneNumber, PhoneNumberViewModel model)
    {
        model.PhoneNumber = phoneNumber;
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
