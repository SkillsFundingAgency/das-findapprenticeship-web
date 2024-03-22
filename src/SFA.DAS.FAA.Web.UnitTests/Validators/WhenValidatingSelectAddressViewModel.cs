using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Validators;
public class WhenValidatingSelectAddressViewModel
{
    private readonly string NoInputErrorMessage = "Select your address or select 'Enter address manually'";

    [Test, MoqAutoData]
    public async Task And_User_Does_Not_Select_Address_Then_Error(
        SelectAddressViewModel model)
    {
        model.SelectedAddress = null;
        var validator = new SelectAddressViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.SelectedAddress)
            .WithErrorMessage(NoInputErrorMessage);
    }

    [Test, MoqAutoData]
    public async Task And_User_Selects_Label_Option_Then_Error(SelectAddressViewModel model)
    {
        model.SelectedAddress = "choose";
        var validator = new SelectAddressViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.SelectedAddress)
            .WithErrorMessage(NoInputErrorMessage);
    }

    [Test, MoqAutoData]
    public async Task And_User_Selects_An_Address_Then_No_Error(SelectAddressViewModel model)
    {
        var validator = new SelectAddressViewModelValidator();

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.SelectedAddress);
    }
}
