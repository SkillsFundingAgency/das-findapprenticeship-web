using System.Text.Json.Serialization;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IBaseApiRequest
    {
        [JsonIgnore]
        string BaseUrl { get; }
    }
}