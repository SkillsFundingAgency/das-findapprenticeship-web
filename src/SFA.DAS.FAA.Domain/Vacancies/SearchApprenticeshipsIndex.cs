using Newtonsoft.Json;


namespace SFA.DAS.FAA.Domain.Vacancies
{
    public class SearchApprenticeshipsIndex
    {
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
