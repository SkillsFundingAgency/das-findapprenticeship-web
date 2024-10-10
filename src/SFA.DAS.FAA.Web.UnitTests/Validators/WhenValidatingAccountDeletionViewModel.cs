using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;

[TestFixture]
public class WhenValidatingAccountDeletionViewModel
{
    private const string EmailAddressEmptyErrorMessage = "Enter your email address";
    private const string EmailAddressFormatErrorMessage = "Enter your email address in the correct format";


    [Test, MoqInlineAutoData("abc@test.com")]
    [MoqInlineAutoData("test@eduction.gov.uk")]
    [MoqInlineAutoData("someTest@test.co.uk")]
    public async Task And_Email_Is_Valid_Then_Validation_Success(string email)
    {
        var model = new AccountDeletionViewModel
        {
            Email = email
        };

        var validator = new AccountDeletionViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().Be(true);
    }

    [Test]
    [MoqInlineAutoData(null, EmailAddressEmptyErrorMessage, false)]
    [MoqInlineAutoData("", EmailAddressEmptyErrorMessage, false)]
    [MoqInlineAutoData("testtest", EmailAddressFormatErrorMessage, false)]
    public async Task And_Email_Invalid_ValidationResult_Is_Invalid(
        string? email,
        string? errorMessage, 
        bool isValid)
    {
        var model = new AccountDeletionViewModel
        {
            Email = email
        };

        var validator = new AccountDeletionViewModelValidator();
        var result = await validator.TestValidateAsync(model);

        result.IsValid.Should().Be(isValid);
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage(errorMessage);
    }
}
