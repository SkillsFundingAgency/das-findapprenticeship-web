using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityEthnicSubGroupViewModelValidator : AbstractValidator<EqualityQuestionsEthnicSubGroupViewModel>
{
    private const string NoEthnicSubGroupSelectionErrorMessage = "Select which of the following best describes your white background, or select 'Prefer not to say'";

    public EqualityEthnicSubGroupViewModelValidator()
    {
        RuleFor(x => x.EthnicSubGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoEthnicSubGroupSelectionErrorMessage)
            .NotEmpty().WithMessage(NoEthnicSubGroupSelectionErrorMessage);
    }
}
