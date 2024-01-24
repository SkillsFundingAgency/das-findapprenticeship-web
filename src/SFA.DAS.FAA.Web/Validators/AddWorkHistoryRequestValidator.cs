using FluentValidation;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Validators
{
    public class AddWorkHistoryRequestValidator : AbstractValidator<AddWorkHistoryRequest>
    {
        private const string VacancyReferenceEmpty = "You must include a vacancy reference.";
        private const string VacancyReferenceTooShort = "The vacancy reference must be atleast 10 characters or more.";
        private const string VacancyReferenceNotNumeric = "The vacancy reference must be a numeric & valid value.";
        private const string VacancyNotSelectAnyJobs = "Select if you want to add any jobs";

        public AddWorkHistoryRequestValidator()
        {
            RuleFor(x => x.VacancyReference)
                .NotEmpty().WithMessage(VacancyReferenceEmpty)
                .NotNull()
                .MinimumLength(10).WithMessage(VacancyReferenceTooShort);

            RuleFor(a => a.VacancyReference).Must(x => int.TryParse(x, out var val) && val > 0)
                .WithMessage(VacancyReferenceNotNumeric);

            RuleFor(r => r.AddJob)
                .NotNull().WithMessage(VacancyNotSelectAnyJobs);
        }
    }
}
