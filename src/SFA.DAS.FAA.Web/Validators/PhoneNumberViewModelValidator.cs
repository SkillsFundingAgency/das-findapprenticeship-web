using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class PhoneNumberViewModelValidator : AbstractValidator<PhoneNumberViewModel>
{
    private readonly string NoInput = "Enter your telephone number";
    private readonly string InvalidInput = "Enter a telephone number, like 01632 960 001, 07700 900 982 or +44 808 157 0192";
    public PhoneNumberViewModelValidator()
    {
        RuleFor(x => x.PhoneNumber).Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(NoInput)
            .Matches(@"^(?:(?:\(?(?:0(?:0|11)\)?[\s-]?\(?|\+)44\)?[\s-]?(?:\(?0\)?[\s-]?)?)|(?:\(?0))(?:(?:\d{5}\)?[\s-]?\d{4,5})|(?:\d{4}\)?[\s-]?(?:\d{5}|\d{3}[\s-]?\d{3}))|(?:\d{3}\)?[\s-]?\d{3}[\s-]?\d{3,4})|(?:\d{2}\)?[\s-]?\d{4}[\s-]?\d{4}))(?:[\s-]?(?:x|ext\.?|\#)\d{3,4})?$")
            .WithMessage(InvalidInput);
    }
}