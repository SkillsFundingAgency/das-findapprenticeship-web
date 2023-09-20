using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex
{
    public class SearchApprenticeshipsIndex
    {
        [JsonProperty("totalApprenticeshipCount")]
        public int Total { get; set; }
    }
}
