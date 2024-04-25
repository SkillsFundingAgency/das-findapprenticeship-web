using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityEthnicSubGroupOtherViewModelValidator : AbstractValidator<EqualityQuestionsEthnicSubGroupOtherViewModel>
{
    private const string NoEthnicSubGroupSelectionErrorMessage = "Select which of the following best describes your background, or select 'Prefer not to say'";

    public EqualityEthnicSubGroupOtherViewModelValidator()
    {
        RuleFor(x => x.EthnicSubGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoEthnicSubGroupSelectionErrorMessage)
            .NotEmpty().WithMessage(NoEthnicSubGroupSelectionErrorMessage);
    }
}