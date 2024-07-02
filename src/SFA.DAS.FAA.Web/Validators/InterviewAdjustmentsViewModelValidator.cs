using System.Data;
using FluentValidation;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.FAA.Web.Validators;

public class InterviewAdjustmentsViewModelValidator : AbstractValidator<InterviewAdjustmentsViewModel>
{
    public InterviewAdjustmentsViewModelValidator()
    {
        RuleFor(x => x.DoYouWantInterviewAdjustments)
        .NotNull()
        .WithMessage("Select if you want to request interview support");

        RuleFor(x => x.InterviewAdjustmentsDescription)
        .NotNull()
        .When(x => x.DoYouWantInterviewAdjustments is true)
        .WithMessage("Enter your interview support request")
        .Must(x => x.GetWordCount() <= 500)
        .WithMessage("Your answer must be 500 words or less");

        RuleFor(x => x.InterviewAdjustmentsDescription)
            .ValidFreeTextCharacters();
    }
}
