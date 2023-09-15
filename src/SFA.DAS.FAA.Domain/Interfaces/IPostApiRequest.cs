using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace SFA.DAS.FAA.Domain.Interfaces;

public interface IPostApiRequest<TData> : IBaseApiRequest
{
    [JsonIgnore]
    string PostUrl { get; }

    TData Data { get; set; }
}