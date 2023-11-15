using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class Vacancies
{
        public int vacancyId { get; private set; }
        [JsonProperty("title")]
        public string title { get; private set; }
        [JsonProperty("employerName")]
        public string employerName { get; private set; }
        [JsonProperty("addressLine4")]
        public string vacancyLocation { get; private set; }
        [JsonProperty("postcode")]
        public string vacancyPostCode { get; private set; }

        public string courseTitle { get; private set; }
        [JsonProperty("apprenticeshipLevel")]
        public string Level { get; private set; }
        [JsonProperty("wage text")]
        public string wage { get; private set; }
        [JsonProperty("closingDate")]
        public DateTime advertClosing { get; private set; }
        [JsonProperty("postedDate")]
         public DateTime postedDate { get; private set; }
}