using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class Vacancies
{
    [JsonProperty("vacancyReference")] 
    public int vacancyReference { get; private set; }
    [JsonProperty("title")] 
    public string title { get; private set; }
    [JsonProperty("employerName")] 
    public string employerName { get; private set; }
    [JsonProperty("addressLine3")] 
    public string addressLine3 { get; private set; }

    [JsonProperty("addressLine4")] 
    public string? addressLine4 { get; private set; }
    [JsonProperty("postcode")] 
    public string vacancyPostCode { get; private set; }
    [JsonProperty("course.title")] 
    public string courseTitle { get; private set; }
    [JsonProperty("wageAmount")] 
    public string wage { get; private set; }
    [JsonProperty("closingDate")] 
    public DateTime closingDate { get; private set; }
    [JsonProperty("postedDate")] 
    public DateTime postedDate { get; private set; }
    [JsonProperty("wageType")] 
    public string wageType { get; private set; }
    [JsonProperty("distance")]
    public double? distance { get; private set; }
}