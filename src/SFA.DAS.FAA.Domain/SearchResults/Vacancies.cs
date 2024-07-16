using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SFA.DAS.FAA.Domain.Enums;

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

    [JsonPropertyName("courseTitle")]
    public string CourseTitle { get; set; }
    [JsonPropertyName("courseId")]
    public int CourseId { get; set; }
    [JsonPropertyName("wageAmount")]
    public string WageAmount { get; set; }
    [JsonPropertyName("wageType")]
    public int WageType { get; set; }

    [JsonPropertyName("addressLine1")]
    public string? AddressLine1 { get; set; }

    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("addressLine3")]
    public string? AddressLine3 { get; set; }

    [JsonPropertyName("addressLine4")]
    public string? AddressLine4 { get; set; }

    [JsonPropertyName("postcode")]
    public string Postcode { get; set; }

    [JsonPropertyName("distance")]
    public decimal? Distance { get; set; }

    [JsonPropertyName("courseLevel")]
    public string CourseLevel { get; set; }

    [JsonPropertyName("vacancyReference")]
    public string VacancyReference { get; set; }

    [JsonPropertyName("wageText")]
    public string WageText { get; set; }

    [JsonPropertyName("applicationStatus")]
    public string ApplicationStatus { get; set; }

    [JsonProperty("isDisabilityConfident")]
    public bool IsDisabilityConfident { get; set; }

    [JsonProperty("isNew")]
    public bool IsNew { get; set; }

    [JsonProperty("isClosingSoon")]
    public bool IsClosingSoon { get; set; }


    [JsonProperty("lat")]
    public double? Lat { get; set; }
    [JsonProperty("lon")]
    public double? Lon { get; set; }

    [JsonProperty("application")]
    public CandidateApplicationDetails? CandidateApplicationDetails { get; set; }

    [JsonProperty("applicationUrl")]
    public string? ApplicationUrl { get; set; }
}

public class CandidateApplicationDetails
{
    [JsonProperty("status")]
    public ApplicationStatus? Status { get; set; }
    
}

