using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Interfaces;

public interface IPostApiRequest
{
    [JsonIgnore]
    string PostUrl { get; }
    object Data { get; set; }
}