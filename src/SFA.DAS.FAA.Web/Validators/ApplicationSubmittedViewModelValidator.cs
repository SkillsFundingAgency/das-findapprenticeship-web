using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class ApplicationSubmittedViewModelValidator : AbstractValidator<ApplicationSubmittedViewModel>
{
    public ApplicationSubmittedViewModelValidator()
    {
        RuleFor(x => x.AnswerEqualityQuestions)
            .NotNull()
            .WithMessage("Select if you want to answer the equality questions");
    }
}
