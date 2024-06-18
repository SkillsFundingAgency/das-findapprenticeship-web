using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;
using System.Net.Mail;

namespace SFA.DAS.FAA.Web.Validators
{
    public class SignInToYourOldAccountViewModelValidator : AbstractValidator<SignInToYourOldAccountViewModel>
    {
        private const string EmailAddressEmptyErrorMessage = "Enter your email address";
        private const string PasswordEmptyErrorMessage = "Enter your password";
        private const string EmailAddressFormatErrorMessage =
            "Enter an email address in the correct format, like name@example.com";

        public SignInToYourOldAccountViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(EmailAddressEmptyErrorMessage);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(PasswordEmptyErrorMessage);

            RuleFor(x => x.Email)
                .Must((model, s) =>
                {
                    try
                    {
                        var emailAddress = new MailAddress(model.Email.Trim());
                        return emailAddress.Address == model.Email.Trim();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                })
                .WithMessage(EmailAddressFormatErrorMessage)
                .When(model => !string.IsNullOrWhiteSpace(model.Email));
        }
    }
}
