namespace SFA.DAS.FAA.Web.Models.User;

public class PostcodeAddressViewModel : ViewModelBase
{
    public bool IsEdit { get; set; }
    public string? Postcode { get; set; }
    public bool? ReturnToConfirmationPage { get; set; }
    public string? BackLink { get; set; }
}
