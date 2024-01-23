using System.Text.Json.Serialization;

namespace SFA.DAS.FAA.Domain.Interfaces;
public interface IPutApiRequest
{
    [JsonIgnore]
    string PutUrl { get; }
    object Data { get; set; }
}
