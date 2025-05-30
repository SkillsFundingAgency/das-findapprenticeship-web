using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;

namespace SFA.DAS.FAA.Web.Models.Apply;

public record EmploymentLocationsSummaryViewModel
{
    public List<EmploymentLocationViewModel> Addresses { get; init; } = [];
    public Guid ApplicationId { get; set; }
    public string? BackLinkUrl { get; set; }

    [BindProperty]
    public bool? IsSectionCompleted { get; set; }

    public static implicit operator EmploymentLocationsSummaryViewModel(GetEmploymentLocationsQueryResult result)
    {
        var addresses = result.EmploymentLocation.Addresses
            .Select(x => (EmploymentLocationViewModel)x)
            .OrderBy(add => add.AddressOrder)
            .ToList();

        return new EmploymentLocationsSummaryViewModel
        {
            Addresses = addresses
        };
    }
}