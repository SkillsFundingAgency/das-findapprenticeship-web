using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User;

public class PhoneNumberViewModel : ViewModelBase
{
    public string? PhoneNumber { get; set; }
    public string BackLink => GetBackLink();
    public string? Postcode { get; set; }
    public UserJourneyPath JourneyPath { get; set; }
    public string RedirectRoute => GetRedirectRoute();
    public string PageTitle => JourneyPath == UserJourneyPath.Settings ? "Change your telephone number – Find an apprenticeship – GOV.UK" : "What is your telephone number? – Find an apprenticeship – GOV.UK";
    public string PageCaption => GetPageCaption();
    public string PageHeading => JourneyPath == UserJourneyPath.Settings ? "Change your telephone number" : "What is your telephone number?";
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.Settings ? "Save" : "Continue";
    
    private string GetBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.AccountFound => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            UserJourneyPath.SelectAddress => RouteNames.SelectAddress,
            UserJourneyPath.EnterAddressManually => RouteNames.EnterAddressManually,
            _ => RouteNames.SelectAddress
        };
    }

    private string GetRedirectRoute()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.AccountFound => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.NotificationPreferences
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
