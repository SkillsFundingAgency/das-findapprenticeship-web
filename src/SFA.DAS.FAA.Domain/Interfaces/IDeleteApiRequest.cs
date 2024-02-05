using System.Text.Json.Serialization;


namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IDeleteApiRequest 
    {
        [JsonIgnore]
        string DeleteUrl { get; }
    }
}
