using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User;

public class PostCodeAddressViewModel : ViewModelBase
{
    [Required(ErrorMessage = "Enter a postcode to search for your address or select 'Enter address manually'")]
    public string? Postcode { get; set; }
}
