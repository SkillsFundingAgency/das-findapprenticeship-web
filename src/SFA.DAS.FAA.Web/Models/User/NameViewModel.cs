using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User;

public class NameViewModel : ViewModelBase
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string BackLink => GetBackLink();
    public UserJourneyPath JourneyPath { get; set; } = UserJourneyPath.CreateAccount;
    public string RedirectRoute => GetRedirectRoute();
    public string PageTitle => JourneyPath == UserJourneyPath.Settings ? "Change your name – Find an apprenticeship – GOV.UK" : "What is your name? – Find an apprenticeship – GOV.UK";
    public string PageCaption => GetPageCaption();
    public string PageHeading => JourneyPath == UserJourneyPath.Settings ? "Change your name" : "What is your name?";
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.Settings ? "Save" : "Continue";

    private string GetBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.CreateAccount => RouteNames.CreateAccount,
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.CreateAccount
        };
    }

    private string GetRedirectRoute()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.DateOfBirth
        };
    }

    private string GetPageCaption()
    {
        return JourneyPath switch
        {
            UserJourneyPath.Settings => string.Empty,
            _ => "Create an account"
        };
    }
}