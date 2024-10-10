using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;
using System.Net.Mail;

namespace SFA.DAS.FAA.Web.Validators
{
    public class AccountDeletionViewModelValidator : AbstractValidator<AccountDeletionViewModel>
    {
        private const string EmailAddressEmptyErrorMessage = "Enter your email address";
        private const string EmailAddressFormatErrorMessage = "Enter your email address in the correct format";

        public AccountDeletionViewModelValidator()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(EmailAddressEmptyErrorMessage);

            RuleFor(x => x.Email)
                .Must((model, _) =>
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
