using FluentValidation;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Validators;

public class LocationViewModelValidator : AbstractValidator<LocationViewModel>
{
    private const string NoNationalSearchErrorMessage = "Select a search location";

    public LocationViewModelValidator()
    {
        RuleFor(x => x.NationalSearch)
            .NotEmpty()
            .WithMessage(NoNationalSearchErrorMessage);
    }
}