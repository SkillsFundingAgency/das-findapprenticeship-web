using CreateAccount.GetAddressesByPostcode;
using SFA.DAS.FAA.Domain.User;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Models.User;

public class SelectAddressViewModel : ViewModelBase
{
    public List<AddressViewModel>? Addresses {  get; set; }
    public string? SelectedAddress { get; set; }
    public string Postcode { get; init; }
    public UserJourneyPath JourneyPath { get; set; } = UserJourneyPath.CreateAccount;
    public string BackLink => GetBackLink();
    public string RedirectRoute => GetRedirectRoute();
    public string PageCaption => GetPageCaption();
    public string PageCtaButtonLabel => JourneyPath == UserJourneyPath.Settings ? "Save" : "Continue";
    private string GetBackLink()
    {
        return JourneyPath switch
        {
            UserJourneyPath.CreateAccount => RouteNames.PostcodeAddress,
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

    public static implicit operator SelectAddressViewModel(GetAddressesByPostcodeQueryResult source)
    {
        return new SelectAddressViewModel
        {
            Postcode = source.Postcode ?? string.Empty,
            SelectedAddress = source.Uprn,
            Addresses = source?.Addresses?.Select(c => (AddressViewModel)c).GroupBy(x => x.AddressLine1).Select(x => x.First()).ToList() ?? new List<AddressViewModel>()
        };
    }
}

public class AddressViewModel
{
    public string Uprn { get; set; }
    public string Organisation { get; set; }
    public string Premises { get; set; }
    public string Thoroughfare { get; set; }
    public string Locality { get; set; }
    public string PostTown { get; set; }
    public string County { get; set; }
    public string Postcode { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Match { get; set; }

    public static implicit operator AddressViewModel(GetAddressesByPostcodeApiResponse.AddressListItem source)
    {
        return new AddressViewModel
        {
            Uprn = source.Uprn,
            Organisation = source.Organisation,
            Premises = source.Premises,
            Thoroughfare = source.Thoroughfare,
            Locality = source.Locality,
            PostTown = source.PostTown,
            County = source.County,
            Postcode = source.Postcode,
            AddressLine1 = source.AddressLine1,
            AddressLine2 = source.AddressLine2,
            AddressLine3 = source.AddressLine3,
            Longitude = source.Longitude,
            Latitude = source.Latitude,
            Match = source.Match
        };
    }
}