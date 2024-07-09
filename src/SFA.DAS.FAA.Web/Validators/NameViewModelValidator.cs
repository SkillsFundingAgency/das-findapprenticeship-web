using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators
{
    public class NameViewModelValidator : AbstractValidator<NameViewModel>
    {
        private const string NoFirstName = "Enter your first name";
        private const string NoLastName = "Enter your last name";

        public NameViewModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage(NoFirstName);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage(NoLastName);
        }
    }
}
