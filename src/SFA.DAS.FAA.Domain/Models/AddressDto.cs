using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Models;

public record AddressDto
{
    [JsonProperty("fullAddress")]
    public string FullAddress { get; set; } = null!;
    [JsonProperty("isSelected")]
    public bool IsSelected { get; set; } = false;
    [JsonProperty("addressOrder")]
    public short AddressOrder { get; set; }
}