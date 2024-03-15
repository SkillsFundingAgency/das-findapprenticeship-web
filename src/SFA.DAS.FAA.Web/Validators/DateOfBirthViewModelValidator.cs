using FluentValidation;
using SFA.DAS.FAA.Web.Models.Custom;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Validators;

public class DateOfBirthViewModelValidator : AbstractValidator<DateOfBirthViewModel>
{
    private readonly string DateOfBirthEmpty = "Enter your date of birth"; // done
    private readonly string DateOfBirthIncomplete = "Date of birth must include a day, month and year";
    private readonly string InvalidYear = "Year must include 4 numbers";
    private readonly string InvalidMonth = "Month must include 2 numbers";
    private readonly string InvalidDay = "Date must include 2 numbers";
    private readonly string DateIsInFuture = "Date of birth must be in the past"; // done
    private readonly string UserIsTooYoung = "You must be older than thirteen to use Find an apprenticeship"; // done

    public DateOfBirthViewModelValidator()
    {
        RuleFor(x => x.DateOfBirth).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(DateOfBirthEmpty)
            .Must(x => x.DateTimeValue < DateTime.Today)
            .WithMessage(DateIsInFuture)
            .Must(x => BeOverThirteen(x))
            .WithMessage(UserIsTooYoung);

        //When(x => x.DateOfBirth is not null, () =>
        //{
        //    RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
        //    .NotEmpty()
        //    .When(x => x.DateOfBirth.DateTimeValue.Value.Day.ToString().Length > 2)
        //    .WithMessage(InvalidDay);

        //    RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
        //     .NotEmpty()
        //     .When(x => x.Day is not null || x.Year is not null)
        //    .WithMessage(DateOfBirthIncomplete)
        //    .When(x => x.Month.ToString().Length != 2)
        //    .WithMessage(InvalidMonth);

        //    RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
        //     .NotEmpty()
        //    .When(x => x.Day is not null || x.Month is not null)
        //    .WithMessage(DateOfBirthIncomplete)
        //    .When(x => x.Year.ToString().Length != 4)
        //    .WithMessage(InvalidYear);


        //});
    }

    protected bool BeOverThirteen(DayMonthYearDate dob)
    {
        var today = DateTime.Today;
        var dobDateTime = new DateTime(dob.DateTimeValue.Value.Year, dob.DateTimeValue.Value.Month, dob.DateTimeValue.Value.Day);
        var ageInDays = today.Subtract(dobDateTime).TotalDays;

        var ageInYears = Math.Round(ageInDays / 365);

        if (ageInYears < 13) return false;

        return true;
    }
}
