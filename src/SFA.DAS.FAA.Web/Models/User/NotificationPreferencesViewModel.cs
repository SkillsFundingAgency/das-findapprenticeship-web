using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User;

public class NotificationPreferencesViewModel : ViewModelBase
{
    public bool? UnfinishedApplicationReminders { get; set; }
    public string BackLink => GetBackLink();
    public UserJourneyPath JourneyPath { get; set; }
    public string RedirectRoute => GetRedirectRoute();
    public string PageTitle => JourneyPath == UserJourneyPath.Settings ? "Change if you get reminders about your unfinished applications – Find an apprenticeship – GOV.UK" : "Get reminders about your unfinished applications – Find an apprenticeship – GOV.UK";
    public string PageCaption => GetPageCaption();
    public string PageHeading => JourneyPath == UserJourneyPath.Settings ? "Change if you get reminders about your unfinished applications" : "Get reminders about your unfinished applications";
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.Settings ? "Save" : "Continue";

    private string GetBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            UserJourneyPath.AccountFound => RouteNames.AccountFoundTermsAndConditions,
            _ => RouteNames.PhoneNumber
        };
    }

    private string GetRedirectRoute()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.AccountFound => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.ConfirmAccountDetails
        };
    }

    private string GetPageCaption()
    {
        return JourneyPath switch
        {
            UserJourneyPath.AccountFound => string.Empty,
            UserJourneyPath.Settings => string.Empty,
            _ => "Create an account"
        };
    }
}
