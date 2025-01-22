using SFA.DAS.FAA.Web.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User;

public class EnterAddressManuallyViewModel
{
    public string? SelectAddressPostcode { get; set; }
    [Required(ErrorMessage = "Enter address line 1, typically the building and street")]
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? TownOrCity { get; set; }
    public string? County { get; set; }
    [Required(ErrorMessage = "Enter postcode")]
    public string? Postcode { get; set; }

    public UserJourneyPath JourneyPath { get; set; } = UserJourneyPath.CreateAccount;
    public string? BackLink { get; set; }
    public string CustomBackLink => GetCustomBackLink();
    public string RedirectRoute => GetRedirectRoute();
    public string PageCaption => GetPageCaption();
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.Settings ? "Save" : "Continue";

    private string GetCustomBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            _ => RouteNames.PostcodeAddress
        };
    }

    private string GetRedirectRoute()
    {
        return JourneyPath switch
        {
            UserJourneyPath.ConfirmAccountDetails => RouteNames.ConfirmAccountDetails,
            UserJourneyPath.Settings => RouteNames.Settings,
            UserJourneyPath.PhoneNumber => RouteNames.PhoneNumber,
            _ => RouteNames.PhoneNumber
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
