using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class AddJobRequestValidator : AbstractValidator<AddJobPostRequest>
    {
        private const string JobTitleErrorMessage = "Enter a job title";
        private const string JobDescriptionErrorMessage = "Enter job desc.";
        private const string EmployerNameErrorMessage = "Enter employer name";
        private const string StartDateErrorMessage = "Enter start date";
        private const string IsCurrentJobErrorMessage = "Tell us if this is your current job";
        private const string EndDateErrorMessage = "Enter end date";


        public AddJobRequestValidator()
        {
            RuleFor(x => x.JobTitle).NotEmpty().WithMessage(JobTitleErrorMessage);
            RuleFor(x => x.JobDescription).NotEmpty().WithMessage(JobDescriptionErrorMessage);
            RuleFor(x => x.EmployerName).NotEmpty().WithMessage(EmployerNameErrorMessage);
            RuleFor(x => x.StartDate).NotEmpty().WithMessage(StartDateErrorMessage);
            RuleFor(x => x.EndDate).NotEmpty().WithMessage(EndDateErrorMessage);
            RuleFor(x => x.IsCurrentRole).NotEmpty().WithMessage(IsCurrentJobErrorMessage);
        }
    }
}
