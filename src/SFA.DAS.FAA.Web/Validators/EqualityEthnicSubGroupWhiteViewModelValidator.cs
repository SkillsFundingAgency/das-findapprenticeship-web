using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityEthnicSubGroupWhiteViewModelValidator : AbstractValidator<EqualityQuestionsEthnicSubGroupWhiteViewModel>
{
    private const string NoEthnicSubGroupSelectionErrorMessage = "Select which of the following best describes your white background, or select 'Prefer not to say'";

    public EqualityEthnicSubGroupWhiteViewModelValidator()
    {
        RuleFor(x => x.EthnicSubGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoEthnicSubGroupSelectionErrorMessage)
            .NotEmpty().WithMessage(NoEthnicSubGroupSelectionErrorMessage);
        
        RuleFor(c => c.OtherEthnicSubGroupAnswer).ValidFreeTextCharacters();
    }
}