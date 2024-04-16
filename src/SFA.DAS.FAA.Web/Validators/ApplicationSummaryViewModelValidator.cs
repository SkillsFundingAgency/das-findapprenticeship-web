using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class ApplicationSummaryViewModelValidator : AbstractValidator<ApplicationSummaryViewModel>
    {
        public ApplicationSummaryViewModelValidator()
        {
            RuleFor(x => x.IsConsentProvided)
                .Must(x => x == true)
                .WithMessage("Select that you understand you won’t be able to make changes after submitting your application");
        }
    }
}
