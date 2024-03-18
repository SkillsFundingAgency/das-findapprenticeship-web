using FluentValidation;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class DateOfBirthViewModelValidator : AbstractValidator<DateOfBirthViewModel>
{
    private readonly string DateOfBirthEmpty = "Enter your date of birth";
    private readonly string DateIsInFuture = "Date of birth must be in the past";
    private readonly string UserIsTooYoung = "You must be older than 13 to use Find an apprenticeship";

    public DateOfBirthViewModelValidator()
    {
        RuleFor(x => x.DateOfBirth).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(DateOfBirthEmpty)
            .Must(x => x.DateTimeValue < DateTime.Today)
            .WithMessage(DateIsInFuture)
            .Must(x => BeOverThirteen(x))
            .WithMessage(UserIsTooYoung);
    }

    protected bool BeOverThirteen(DayMonthYearDate dob)
    {
        var dobDateTime = new DateTime(dob.DateTimeValue.Value.Year, dob.DateTimeValue.Value.Month, dob.DateTimeValue.Value.Day);
        var date13YearsAgo = DateTime.Today.AddYears(-13);

        if (dobDateTime <= date13YearsAgo) return true;

        return false;
    }
}
