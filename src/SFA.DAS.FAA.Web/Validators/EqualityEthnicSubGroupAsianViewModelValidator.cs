﻿using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators;

public class EqualityEthnicSubGroupAsianViewModelValidator : AbstractValidator<EqualityQuestionsEthnicSubGroupAsianViewModel>
{
    private const string NoEthnicSubGroupSelectionErrorMessage = "Select which of the following best describes your Asian or Asian British background, or select 'Prefer not to say'";

    public EqualityEthnicSubGroupAsianViewModelValidator()
    {
        RuleFor(x => x.EthnicSubGroup).Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(NoEthnicSubGroupSelectionErrorMessage)
            .NotEmpty().WithMessage(NoEthnicSubGroupSelectionErrorMessage);
    }
}