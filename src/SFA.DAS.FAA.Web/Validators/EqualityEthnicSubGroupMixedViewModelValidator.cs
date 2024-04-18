using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityEthnicSubGroupMixedViewModelValidator : AbstractValidator<EqualityQuestionsEthnicSubGroupMixedViewModel>
{
    private const string NoEthnicSubGroupSelectionErrorMessage = "Select which of the following best describes your mixed or multiple ethnic groups background, or select 'Prefer not to say'";

    public EqualityEthnicSubGroupMixedViewModelValidator()
    {
        RuleFor(x => x.EthnicSubGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoEthnicSubGroupSelectionErrorMessage)
            .NotEmpty().WithMessage(NoEthnicSubGroupSelectionErrorMessage);
    }
}
