using FluentValidation;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.FAA.Web.Validators;

public class SkillsAndStrengthsViewModelValidator : AbstractValidator<SkillsAndStrengthsViewModel>
{
    private const string SkillsAndStrengthsEmptyErrorMessage = "Enter your skills and strengths - you must enter an answer before making this section complete";
    private const string SkillsAndStrengthsOverMaxLengthErrorMessage = "Your skills and strengths must be 300 words or less";
    private const string IsSectionCompleteNotSelectedErrorMessage = "Select if you have finished this section";
    public SkillsAndStrengthsViewModelValidator()
    {
        When(x => x.AutoSave is false, () =>
            {
                RuleFor(x => x.IsSectionComplete)
                    .NotNull().WithMessage(IsSectionCompleteNotSelectedErrorMessage);
            }
        );

        RuleFor(x => x.SkillsAndStrengths).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(x => x.IsSectionComplete.HasValue && x.IsSectionComplete.Value is true)
            .WithMessage(SkillsAndStrengthsEmptyErrorMessage);

        RuleFor(x => x.SkillsAndStrengths)
            .Must(x => x.GetWordCount() <= 300)
            .WithMessage(SkillsAndStrengthsOverMaxLengthErrorMessage);

        RuleFor(x => x.SkillsAndStrengths)
            .ValidFreeTextCharacters();
    }
}