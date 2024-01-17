using FluentValidation;
using SFA.DAS.FAA.Web.Models.Vacancy;

namespace SFA.DAS.FAA.Web.Validators
{
    public class GetVacancyDetailsRequestValidator : AbstractValidator<GetVacancyDetailsRequest>
    {
        public const string VacancyReferenceEmpty = "You must include a vacancy reference.";
        public const string VacancyReferenceTooShort = "The vacancy reference must be atleast 10 characters or more.";
        public const string VacancyReferenceNotNumeric = "The vacancy reference must be a numeric & valid value.";

        public GetVacancyDetailsRequestValidator()
        {
            RuleFor(x => x.VacancyReference)
                .NotEmpty().WithMessage(VacancyReferenceEmpty)
                .NotNull()
                .MinimumLength(10).WithMessage(VacancyReferenceTooShort);

            RuleFor(a => a.VacancyReference).Must(x => int.TryParse(x, out var val) && val > 0)
                .WithMessage(VacancyReferenceNotNumeric);
        }
    }
}
