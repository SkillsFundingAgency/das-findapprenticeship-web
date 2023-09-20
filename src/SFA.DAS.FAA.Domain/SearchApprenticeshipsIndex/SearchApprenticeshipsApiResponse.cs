using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex
{
    public class SearchApprenticeshipsApiResponse
    {
        [JsonProperty("totalApprenticeshipCount")]
        public int Total { get; set; }
    }
}
