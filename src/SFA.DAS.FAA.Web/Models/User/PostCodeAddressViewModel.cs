using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User;

public class PostcodeAddressViewModel : ViewModelBase
{
    public string? Postcode { get; set; }
    public UserJourneyPath JourneyPath { get; set; } = UserJourneyPath.CreateAccount;
    public string? BackLink => GetBackLink();
    private string GetBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.DateOfBirth
        };
    }
}