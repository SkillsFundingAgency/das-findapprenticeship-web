namespace SFA.DAS.FAA.Web.Models.User;

public class PostcodeAddressViewModel : ViewModelBase
{
    public string? Postcode { get; set; }
    public bool? ReturnToConfirmationPage { get; set; }
    public string BackLink { get; set; }
}
