using FluentValidation;
using SFA.DAS.FAA.Web.Models.Vacancy;

namespace SFA.DAS.FAA.Web.Validators;

public class GetVacancyDetailsRequestValidator : AbstractValidator<GetVacancyDetailsRequest>
{
    private const string VacancyReferenceEmpty = "You must include a vacancy reference.";
    private const string VacancyReferenceTooShort = "The vacancy reference must be atleast 7 characters or more.";

    public GetVacancyDetailsRequestValidator()
    {
        RuleFor(x => x.VacancyReference)
            .NotEmpty().WithMessage(VacancyReferenceEmpty)
            .NotNull()
            .MinimumLength(7).WithMessage(VacancyReferenceTooShort);
    }
}