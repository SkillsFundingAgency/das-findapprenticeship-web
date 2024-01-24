using System.Text.Json.Serialization;

namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IPatchApiRequest
    {
        [JsonIgnore]
        string PatchUrl { get; }

        object Data { get; set; }
    }
}
