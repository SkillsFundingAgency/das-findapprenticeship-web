using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class Vacancies
{
    [JsonProperty("vacancyReference")] 
    public int VacancyReference { get; set; }
    [JsonProperty("title")] 
    public string Title { get; set; }
    [JsonProperty("employerName")] 
    public string EmployerName { get; set; }
    [JsonProperty("closingDate")] 
    public DateTime ClosingDate { get; set; }
    [JsonProperty("postedDate")] 
    public DateTime PostedDate { get; set; }
    [JsonProperty("distance")]
    public double? Distance { get; set; }
    [JsonProperty("course")]
    public Course Course { get; set; }
    [JsonProperty("wage")]
    public Wage Wage { get; set; }
    [JsonProperty("address")]
    public Address Address { get; set; }

}

public class Course
{
    public string Title { get; set; }
    public int Level { get; set; }
    public string Route { get; set; }
}

public class Wage
{
    public double? WageAmount { get; set; }
    public string WageType { get; set; }
    public string WageUnit { get; set; }
}

public class Address
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string Postcode { get; set; }
}