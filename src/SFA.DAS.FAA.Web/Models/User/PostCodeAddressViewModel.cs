using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User;

public class PostcodeAddressViewModel : ViewModelBase
{
    public string? Postcode { get; set; }
}
