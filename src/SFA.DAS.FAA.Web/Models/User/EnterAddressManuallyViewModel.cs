using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.User;

public class EnterAddressManuallyViewModel
{
    public bool IsEdit { get; set; }
    public string BackLink { get; set; }
    public string? SelectAddressPostcode { get; set; }
    [Required(ErrorMessage = "Enter address line 1, typically the building and street")]
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    [Required(ErrorMessage = "Enter town or city")]
    public string? TownOrCity { get; set; }
    public string? County { get; set; }
    [Required(ErrorMessage = "Enter postcode")]
    public string? Postcode { get; set; }
    public bool? ReturnToConfirmationPage { get; set; }
}
