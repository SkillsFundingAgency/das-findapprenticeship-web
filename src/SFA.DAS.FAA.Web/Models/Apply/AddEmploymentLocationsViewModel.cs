using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record AddEmploymentLocationsViewModel
{
    public List<EmploymentLocationViewModel> Addresses { get; init; } = [];
    public Guid ApplicationId { get; set; }
    public List<Guid>? SelectedAddressIds { get; init; }

    public static implicit operator AddEmploymentLocationsViewModel(GetEmploymentLocationsQueryResult result)
    {
        var addresses = result.EmploymentLocation.Addresses
            .Select(x => (EmploymentLocationViewModel)x)
            .OrderBy(add => add.AddressOrder)
            .ToList();

        return new AddEmploymentLocationsViewModel
        {
            Addresses = addresses
        };
    }
}