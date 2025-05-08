using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record AddEmploymentLocationsViewModel
{
    public List<AddressDto> Addresses { get; set; } = [];
    public Guid ApplicationId { get; set; }
    public List<Guid>? SelectedAddressIds { get; set; }
}