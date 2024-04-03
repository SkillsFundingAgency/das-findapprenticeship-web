using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.User;

public class DateOfBirthViewModel
{
    public DayMonthYearDate? DateOfBirth { get; set; }
    public bool? ReturnToConfirmationPage { get; set; }
    public string BackLink { get; set; }
}
