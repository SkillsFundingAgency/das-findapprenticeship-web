using FluentValidation;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class AddAdditionalQuestionViewModelValidator : AbstractValidator<AddAdditionalQuestionViewModel>
{
    public AddAdditionalQuestionViewModelValidator()
    {
        RuleFor(x => x.AdditionalQuestionAnswer).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Enter your answer – you must enter an answer before making this section complete")
            .Must(x => x.GetWordCount() <= 300)
            .WithMessage("Your answer must be 300 words or less");

        RuleFor(x => x.IsSectionCompleted)
        .NotNull()
        .WithMessage("Select if you have finished this section");
    }
}
