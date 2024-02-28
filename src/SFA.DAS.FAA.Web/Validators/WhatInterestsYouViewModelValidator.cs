using FluentValidation;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class WhatInterestsYouViewModelValidator : AbstractValidator<WhatInterestsYouViewModel>
    {
        public WhatInterestsYouViewModelValidator()
        {
            RuleFor(x => x.IsSectionCompleted)
                .NotEmpty()
                .WithMessage("Select if you have finished this section");

            RuleFor(x => x.YourInterest)
                .Must((model, s) => !string.IsNullOrWhiteSpace(model.YourInterest))
                .When(x => x.IsSectionCompleted is true)
                .WithMessage("Enter your interest in this apprenticeship – you must enter an answer before making this section complete");

            RuleFor(x => x.YourInterest).Cascade(CascadeMode.Stop)
                .Must(x => x.GetWordCount() <= 300)
                .WithMessage("Your interest in this apprenticeship must be 300 words or less");
        }
    }
}
