using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex
{
    public class SearchApprenticeshipsIndex
    {
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
