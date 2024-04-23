using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityEthnicGroupViewModelValidator : AbstractValidator<EqualityQuestionsEthnicGroupViewModel>
{
    private const string NoEthnicGroupSelectionErrorMessage = "Select an ethnic group or 'Prefer not to say'";

    public EqualityEthnicGroupViewModelValidator()
    {
        RuleFor(x => x.EthnicGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoEthnicGroupSelectionErrorMessage)
            .NotEmpty().WithMessage(NoEthnicGroupSelectionErrorMessage);
    }
}
