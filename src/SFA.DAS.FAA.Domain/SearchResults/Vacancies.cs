﻿using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class Vacancies
{
    [JsonProperty("vacancyReference")] 
    public int vacancyReference { get; private set; }
    [JsonProperty("title")] 
    public string title { get; private set; }
    [JsonProperty("employerName")] 
    public string employerName { get; private set; }
    [JsonProperty("closingDate")] 
    public DateTime closingDate { get; private set; }
    [JsonProperty("postedDate")] 
    public DateTime postedDate { get; private set; }
    [JsonProperty("distance")]
    public double? distance { get; private set; }
    [JsonProperty("course")]
    public Course course { get; set; }
    [JsonProperty("wage")]
    public Wage wage { get; set; }
    [JsonProperty("address")]
    public Address address { get; set; }

}

public class Course
{
    public int larsCode { get; set; }
    public string title { get; set; }
    public int level { get; set; }
    public string route { get; set; }
}

public class Wage
{
    public double wageAmount { get; set; }
    public string wageAdditionalInformation { get; set; }
    public string wageType { get; set; }
    public string workingWeekDescription { get; set; }
    public string wageUnit { get; set; }
}

public class Address
{
    public string addressLine1 { get; set; }
    public string addressLine3 { get; set; }
    public string? addressLine4 { get; set; }
    public string postcode { get; set; }
}