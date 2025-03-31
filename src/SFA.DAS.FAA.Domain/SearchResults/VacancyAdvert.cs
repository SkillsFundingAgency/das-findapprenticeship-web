using Newtonsoft.Json;
using System.Text.Json.Serialization;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.SearchResults;

public class VacancyAdvert : IVacancyAdvert
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("apprenticeshipLevel")]
    public string ApprenticeshipLevel { get; set; }

    [JsonPropertyName("closingDate")]
    public DateTime ClosingDate { get; set; }
    
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

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
    [JsonPropertyName("over25NationalMinimumWage")]
    public decimal? Over25NationalMinimumWage { get; set; }
    [JsonPropertyName("between21AndUnder25NationalMinimumWage")]
    public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    [JsonPropertyName("between18AndUnder21NationalMinimumWage")]
    public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    [JsonPropertyName("under18NationalMinimumWage")]
    public decimal? Under18NationalMinimumWage { get; set; }
    [JsonPropertyName("apprenticeMinimumWage")]
    public decimal? ApprenticeMinimumWage { get; set; }

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

    [JsonPropertyName("employmentLocationInformation")]
    public string? EmploymentLocationInformation { get; set; }

    [JsonPropertyName("distance")]
    public decimal? Distance { get; set; }

    [JsonPropertyName("otherAddresses")]
    public List<Address>? OtherAddresses { get; set; }

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

    [JsonProperty("isSavedVacancy")]
    public bool IsSavedVacancy { get; set; }

    [JsonPropertyName("vacancySource")]
    public VacancyDataSource VacancySource { get; set; }
}

public class CandidateApplicationDetails
{
    [JsonProperty("status")]
    public ApplicationStatus? Status { get; set; }
}


