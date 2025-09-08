using FluentValidation;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Validators;

public class LocationViewModelValidator : AbstractValidator<LocationViewModel>
{
    private const string NoNationalSearchErrorMessage = "Select if you want to enter a city or postcode or if you want to search across all of England";

    public LocationViewModelValidator()
    {
        RuleFor(x => x.NationalSearch)
            .NotEmpty()
            .WithMessage(NoNationalSearchErrorMessage);
    }
}