using System.Text.Json.Serialization;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class Vacancies
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("apprenticeshipLevel")]
    public string ApprenticeshipLevel { get; set; }

    [JsonPropertyName("closingDate")]
    public DateTime ClosingDate { get; set; }

    [JsonPropertyName("employerName")]
    public string EmployerName { get; set; }

    [JsonPropertyName("postedDate")]
    public DateTime PostedDate { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("vacancyReference")]
    public string VacancyReference { get; set; }

    [JsonPropertyName("subCategory")]
    public string CourseTitle { get; set; }
    [JsonPropertyName("standardLarsCode")]
    public int CourseId { get; set; }
    [JsonPropertyName("wageText")]
    public string WageAmount { get; set; }
    [JsonPropertyName("wageType")]
    public int WageType { get; set; }

    [JsonPropertyName("address")]
    public Address Address { get; set; }

    [JsonPropertyName("distance")]
    public decimal? Distance { get; set; }
}

public class Address
{
    [JsonPropertyName("addressLine1")]
    public string AddressLine1 { get; set; }
    [JsonPropertyName("addressLine2")]
    public string AddressLine2 { get; set; }
    [JsonPropertyName("addressLine3")]
    public string AddressLine3 { get; set; }
    [JsonPropertyName("addressLine4")]
    public string AddressLine4 { get; set; }
    [JsonPropertyName("postcode")]
    public string Postcode { get; set; }
}