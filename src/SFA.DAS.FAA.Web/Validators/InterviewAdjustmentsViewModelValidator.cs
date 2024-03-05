using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class InterviewAdjustmentsViewModelValidator : AbstractValidator<InterviewAdjustmentsViewModel>
{
    public InterviewAdjustmentsViewModelValidator()
    {
        RuleFor(x => x.DoYouWantInterviewAdjustments)
        .NotNull()
        .WithMessage("Select if you want to request interview support");
    }
}
