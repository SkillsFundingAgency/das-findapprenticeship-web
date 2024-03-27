using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class DisabilityConfidentSummaryViewModelValidator : AbstractValidator<DisabilityConfidentSummaryViewModel>
    {
        public DisabilityConfidentSummaryViewModelValidator()
        {
            RuleFor(x => x.IsSectionCompleted)
                .NotNull()
                .WithMessage("Select if you have completed this section");
        }
    }
}
