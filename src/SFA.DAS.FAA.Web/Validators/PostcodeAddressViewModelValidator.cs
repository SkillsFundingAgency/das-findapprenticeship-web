using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class PostcodeAddressViewModelValidator : AbstractValidator<PostcodeAddressViewModel>
{
    private readonly string IllegalCharacters = "A postcode can only have letters, numbers, and spaces.";
    private readonly string MaximumLength = "Enter 10 or less characters";
    private readonly string PostcodeRequired = "Enter a postcode to search for your address or select 'Enter address manually'";
    public PostcodeAddressViewModelValidator()
    {
        RuleFor(x => x.Postcode).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(PostcodeRequired)
            .MaximumLength(10)
            .WithMessage(MaximumLength)
            .Matches(@"^[a-zA-Z0-9\s]*$")
            .WithMessage(IllegalCharacters);
    }
}
