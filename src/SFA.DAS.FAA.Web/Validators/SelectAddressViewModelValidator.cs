using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class SelectAddressViewModelValidator : AbstractValidator<SelectAddressViewModel>
{
    private readonly string NoAddressSelected = "Select your address or select 'Enter my address manually'";
    public SelectAddressViewModelValidator()
    {
        RuleFor(x => x.SelectedAddress)
            .NotNull()
            .WithMessage(NoAddressSelected)
            .NotEqual("choose")
            .WithMessage(NoAddressSelected);
    }
}
