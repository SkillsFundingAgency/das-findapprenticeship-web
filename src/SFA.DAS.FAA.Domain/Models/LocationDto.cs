using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Models;

public record LocationDto
{
    [JsonProperty("addresses")]
    public List<AddressDto> Addresses { get; set; } = null!;
    [JsonProperty("employerLocationOption")]
    public AvailableWhere? EmployerLocationOption { get; set; }
    [JsonProperty("employmentLocationInformation")]
    public string? EmploymentLocationInformation { get; set; }
}