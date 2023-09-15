using System.Text.Json.Serialization;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IDeleteApiRequest : IBaseApiRequest
    {
        [JsonIgnore]
        string DeleteUrl { get; }
    }
}