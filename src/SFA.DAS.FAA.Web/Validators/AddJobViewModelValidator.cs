using FluentValidation;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Validators
{
    public class AddJobViewModelValidator : AbstractValidator<AddJobViewModel>
    {
        public AddJobViewModelValidator(IDateTimeService dateTimeService)
        {
            Include(new JobViewModelBaseValidator(dateTimeService));
        }
    }

    public class EditJobViewModelValidator : AbstractValidator<EditJobViewModel>
    {
        public EditJobViewModelValidator(IDateTimeService dateTimeService)
        {
            Include(new JobViewModelBaseValidator(dateTimeService));
        }
    }

    public class JobViewModelBaseValidator : AbstractValidator<JobViewModelBase>
    {
        private const string JobTitleErrorMessage = "Enter the job title for this job";
        private const string EmployerNameErrorMessage = "Enter the company you worked for";
        private const string JobDescriptionErrorMessage = "Enter the responsibilities you had for this job";
        private const string JobDescriptionMaxLengthErrorMessage = "Responsibilities must be 100 words or less";
        private const string StartDateErrorMessage = "Enter the start date for this job";
        private const string StartDateIsInThePastErrorMessage = "The start date must be in the past";
        private const string IsCurrentJobErrorMessage = "Select if this is your current job";
        private const string EndDateErrorMessage = "Enter the end date for this job";
        private const string EndDateIsInThePastErrorMessage = "The end date must be in the past";

        public JobViewModelBaseValidator(IDateTimeService dateTimeService)
        {
            RuleFor(x => x.JobTitle).NotEmpty().WithMessage(JobTitleErrorMessage);
            
            RuleFor(x => x.EmployerName).NotEmpty().WithMessage(EmployerNameErrorMessage);
            
            RuleFor(x => x.JobDescription).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(JobDescriptionErrorMessage)
                .Must(x => x.GetWordCount() <= 100)
                .WithMessage(JobDescriptionMaxLengthErrorMessage);
            
            RuleFor(x => x.StartDate).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StartDateErrorMessage)
                .Must(x => x != null && x.DateTimeValue.HasValue && x.DateTimeValue.Value <= dateTimeService.GetDateTime())
                .WithMessage(StartDateIsInThePastErrorMessage);

            RuleFor(x => x.EndDate).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(EndDateErrorMessage)
                .When(x => x.IsCurrentRole.HasValue && x.IsCurrentRole.Value == false)
                .WithMessage(EndDateErrorMessage)
                .Must(x => x != null && x.DateTimeValue.HasValue &&
                           x.DateTimeValue.Value <= dateTimeService.GetDateTime())
                .WithMessage(EndDateIsInThePastErrorMessage)
                .When(x => x.IsCurrentRole.HasValue && x.IsCurrentRole.Value == false);

            RuleFor(x => x.IsCurrentRole).NotEmpty().WithMessage(IsCurrentJobErrorMessage);
        }
    }
}
