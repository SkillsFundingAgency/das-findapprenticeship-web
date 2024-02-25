using FluentValidation;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Models.Apply.Base;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Validators
{
    public class EditVolunteeringAndWorkExperienceViewModelValidator : AbstractValidator<EditVolunteeringAndWorkExperienceViewModel>
    {
        public EditVolunteeringAndWorkExperienceViewModelValidator(IDateTimeService dateTimeService)
        {
            Include(new EditVolunteeringAndWorkExperienceViewModelBaseValidator(dateTimeService));
        }
    }

    public class EditVolunteeringAndWorkExperienceViewModelBaseValidator : AbstractValidator<VolunteeringAndWorkExperienceViewModelBase>
    {
        private const string CompanyNameErrorMessage = "Enter the company or organisation for this volunteering or work experience";
        private const string JobDescriptionErrorMessage = "Enter what you did for this volunteering or work experience";
        private const string JobDescriptionMaxLengthErrorMessage = "What you did must be 100 words or less";
        private const string StartDateErrorMessage = "Enter a real date for the start date";
        private const string StartDateIsInThePastErrorMessage = "The start date must be in the past";
        private const string IsCurrentJobErrorMessage = "Select if you’re still doing this volunteering or work experience";
        private const string EndDateErrorMessage = "Enter a real date for the end date";
        private const string EndDateIsInThePastErrorMessage = "The end date must be in the past";
        private const string EndDateMustBeGreaterThanStartDate = "The end date must be after the start date";

        public EditVolunteeringAndWorkExperienceViewModelBaseValidator(IDateTimeService dateTimeService)
        {
            RuleFor(x => x.CompanyName).NotEmpty().WithMessage(CompanyNameErrorMessage);
            
            RuleFor(x => x.Description).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(JobDescriptionErrorMessage)
                .Must(x => x.GetWordCount() <= 100)
                .WithMessage(JobDescriptionMaxLengthErrorMessage);
            
            RuleFor(x => x.StartDate).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StartDateErrorMessage)
                .Must(x => x is {DateTimeValue: not null} && x.DateTimeValue.Value <= dateTimeService.GetDateTime())
                .WithMessage(StartDateIsInThePastErrorMessage);

            RuleFor(x => x.EndDate).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(EndDateErrorMessage)
                .When(x => x.IsCurrentRole is false)
                .WithMessage(EndDateErrorMessage)
                .Must(x => x is {DateTimeValue: not null} &&
                           x.DateTimeValue.Value <= dateTimeService.GetDateTime())
                .WithMessage(EndDateIsInThePastErrorMessage)
                .When(x => x.IsCurrentRole is false);

            RuleFor(x => x).Cascade(CascadeMode.Stop)
                .Must(x => x.EndDate?.DateTimeValue == default(DateTime) || x.StartDate?.DateTimeValue == default(DateTime) || x.EndDate?.DateTimeValue > x.StartDate?.DateTimeValue)
                .WithMessage(EndDateMustBeGreaterThanStartDate)
                .When(x => x.IsCurrentRole is false)
                .WithName(nameof(VolunteeringAndWorkExperienceViewModelBase.EndDate));

            RuleFor(x => x.IsCurrentRole).NotEmpty().WithMessage(IsCurrentJobErrorMessage);
        }
    }
}
