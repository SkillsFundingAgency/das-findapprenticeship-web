using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;

public record GetEmploymentLocationsApiResponse
{
    [JsonProperty("employmentLocation")] 
    public LocationDto EmploymentLocation { get; private init; } = new();

    public bool? IsSectionCompleted { get; set; }
}