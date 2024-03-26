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
            .Matches(@"^(?:(?:\+?44\s?|0)(?:11|2\d|3[0-9]|4[0-8]|5[0-5]|7[0-9])\s?\d{4}\s?\d{3,4})$")
            .WithMessage(InvalidInput);
    }
}
