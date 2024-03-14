using FluentValidation;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class DateOfBirthViewModelValidator : AbstractValidator<DateOfBirthViewModel>
{
    const string DateOfBirthEmpty = "Enter your date of birth";
    const string DateOfBirthIncomplete = "Date of birth must include a day, month and year";
    const string InvalidYear = "Year must include 4 numbers";
    const string InvalidMonth = "Month must include 2 numbers";
    const string InvalidDay = "Date must include 2 numbers";
    const string DateIsInFuture = "Date of birth must be in the past";

    public DateOfBirthViewModelValidator()
    {
        // no input

        RuleFor(x => x.Day)
            .NotEmpty()
            .When(x => x.Day is null && x.Month is null && x.Year is null)
            .WithMessage(DateOfBirthEmpty);


        // incomplete input or invalid input

        RuleFor(x => x.Day)
            .NotEmpty()
            .When(x => x.Month is not null || x.Year is not null)
            .WithMessage(DateOfBirthIncomplete)
            .When(x => x.Day.ToString().Length > 2)
            .WithMessage(InvalidDay);

        RuleFor(x => x.Month)
            .NotEmpty()
            .When(x => x.Day is not null || x.Year is not null)
            .WithMessage(DateOfBirthIncomplete)
            .When(x => x.Month.ToString().Length != 2)
            .WithMessage(InvalidMonth);

        RuleFor(x => x.Year)
            .NotEmpty()
            .When(x => x.Day is not null || x.Month is not null)
            .WithMessage(DateOfBirthIncomplete)
            .When(x => x.Year.ToString().Length != 4)
            .WithMessage(InvalidYear);

        // user is too young

        RuleFor(x => x.Day)
            .NotNull()
            .When(x => x.Day is not null && x.Month is not null && x.Year is not null && IsOlderThanThirteen(x.Year, x.Month, x.Day) is false)
            .WithMessage("You must be older than 13 to use Find an apprenticeship");

        // date is in future

        RuleFor(x => x.Day)
            .NotNull()
            .When(x => x.Day is not null && x.Month is not null && x.Year is not null
            && new DateTime((int)x.Year, (int)x.Month, (int)x.Day) > DateTime.Today)
            .WithMessage(DateIsInFuture);
    }

    private bool IsOlderThanThirteen(int? day, int? month, int? year)
    {
        var today = DateTime.Today;
        var dob = new DateTime((int)year, (int)month, (int)day);
        var ageInDays = today.Subtract(dob).TotalDays;

        var ageInYears = Math.Round(ageInDays / 365);

        if (ageInYears < 13) return false;

        return true;
    }
}
