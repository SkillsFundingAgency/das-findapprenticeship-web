using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.User;

public class DateOfBirthViewModel
{
    public DayMonthYearDate? DateOfBirth { get; init; }
    public string BackLink => GetBackLink();
    public UserJourneyPath JourneyPath { get; set; } = UserJourneyPath.CreateAccount;
    public string RedirectRoute => GetRedirectRoute();
    public string PageTitle => JourneyPath == UserJourneyPath.Settings ? "Change your date of birth – Find an apprenticeship – GOV.UK" : "Date of birth – Find an apprenticeship – GOV.UK";
    public string PageCaption => JourneyPath == UserJourneyPath.Settings ? string.Empty : "Create an account";
    public string PageHeading => JourneyPath == UserJourneyPath.Settings ? "Change your date of birth" : "Date of birth";
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.Settings ? "Save" : "Continue";

    private string GetBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.CreateAccount => RouteNames.UserName,
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.UserName
        };
    }

    private string GetRedirectRoute()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.PostcodeAddress
        };
    }
}
