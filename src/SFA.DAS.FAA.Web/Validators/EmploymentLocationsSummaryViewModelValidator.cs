using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class EmploymentLocationsSummaryViewModelValidator : AbstractValidator<EmploymentLocationsSummaryViewModel>
    {
        public EmploymentLocationsSummaryViewModelValidator()
        {
            RuleFor(x => x.IsSectionCompleted)
                .NotNull()
                .WithMessage("Select if you have completed this section");
        }
    }
}