using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class InterviewAdjustmentsSummaryViewModelValidator : AbstractValidator<InterviewAdjustmentSummaryViewModel>
    {
        public InterviewAdjustmentsSummaryViewModelValidator()
        {
            RuleFor(x => x.IsSectionCompleted)
                .NotNull()
                .WithMessage("Select if you have completed this section");
        }
    }
}
