namespace SFA.DAS.FAA.Web.Models.User;

public class PhoneNumberViewModel : ViewModelBase
{
    public string? PhoneNumber { get; set; }
    public bool ReturnToConfirmationPage { get; set; }
    public string BackLink { get; set; } = null!;
    public string? Postcode { get; set; }
}
