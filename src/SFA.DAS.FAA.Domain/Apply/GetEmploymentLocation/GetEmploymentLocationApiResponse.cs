using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.Apply.GetEmploymentLocation;

public record GetEmploymentLocationApiResponse
{
    [JsonProperty("employmentLocation")] public LocationDto EmploymentLocation { get; private init; } = new();
}