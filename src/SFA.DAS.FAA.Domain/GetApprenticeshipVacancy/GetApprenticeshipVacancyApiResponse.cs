using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;

[ApiType("GetApprenticeshipVacancyApiResponse")]
public class GetApprenticeshipVacancyApiResponse : IVacancyAdvert
{
    public string? Id { get; init; }
    public string? Title { get; init; }
    public string? VacancyReference { get; init; }
    public string? LongDescription { get; init; }
    public string? OutcomeDescription { get; init; }
    public string? TrainingDescription { get; init; }
    public string? ThingsToConsider { get; init; }
    public IEnumerable<string>? Skills { get; init; }

    public DateTime StartDate { get; set; }
    public DateTime ClosingDate { get; set; }
    public DateTime PostedDate { get; init; }
    public DateTime? WithdrawnDate { get; init; }


    public string? WageAmount { get; init; }
    public int WageType { get; set; }
    public decimal? WageAmountLowerBound { get; init; }
    public decimal? WageAmountUpperBound { get; init; }
    public string? WageText { get; set; }
    public int WageUnit { get; init; }
    public string WageAdditionalInformation { get; set; }

    public decimal? Distance { get; init; }

    public string? CourseTitle { get; init; }
    public string? CourseLevel { get; init; }
    public int CourseId { get; init; }
    public string? CourseRoute { get; init; }
    public string? ApprenticeshipLevel { get; init; }

    public string? Category { get; init; }
    public string? CategoryCode { get; init; }
    public string? SubCategory { get; init; }
    public string? SubCategoryCode { get; init; }

    public string? Description { get; init; }
    public string? FrameworkLarsCode { get; init; }
    public decimal? HoursPerWeek { get; set; }
    public bool IsDisabilityConfident { get; init; }
    public bool IsPositiveAboutDisability { get; init; }
    public bool IsRecruitVacancy { get; init; }
    public Location? Location { get; set; }
    public int NumberOfPositions { get; init; }
    public string? ProviderName { get; init; }
    public int? StandardLarsCode { get; init; }


    public VacancyLocationType? VacancyLocationType { get; init; }

    public string? WorkingWeek { get; init; }
    public string? ExpectedDuration { get; init; }
    public double Score { get; init; }

    public string Ukprn { get; init; }
    public string? EmployerName { get; init; }
    public string? EmployerDescription { get; init; }
    public string? EmployerWebsiteUrl { get; init; }
    public string? EmployerContactPhone { get; init; }
    public string? EmployerContactEmail { get; init; }
    public string? EmployerContactName { get; init; }
    public string? ProviderContactPhone { get; init; }
    public string? ProviderContactEmail { get; init; }
    public string? ProviderContactName { get; init; }
    public string? AnonymousEmployerName { get; init; }
    public bool IsEmployerAnonymous { get; set; }
    public bool IsClosed { get; set; }

    public IEnumerable<VacancyQualificationApiResponse>? Qualifications { get; init; }
    public Address? Address { get; set; }
    public List<Address> OtherAddresses { get; set; } = [];
    public string? EmploymentLocationInformation { get; set; }
    public AvailableWhere? EmployerLocationOption { get; set; }
    public List<string>? CourseSkills { get; init; }
    public List<string>? CourseCoreDuties { get; init; }
    public string? CourseOverviewOfRole { get; init; }
    public string? StandardPageUrl { get; init; }
    public string? CompanyBenefitsInformation { get; init; }
    public string? AdditionalTrainingDescription { get; init; }

    public List<LevelResponse>? Levels { get; init; }

    public CandidateApplicationDetails? Application { get; set; }
    public string? ApplicationUrl { get; set; }
    public string? ApplicationInstructions { get; set; }
    public string? CandidatePostcode { get; set; }
    public bool IsSavedVacancy { get; set; } = false;
    public VacancyDataSource VacancySource { get; set; }
    public decimal? Over25NationalMinimumWage { get; set; }
    public decimal? Between21AndUnder25NationalMinimumWage { get; set; }
    public decimal? Between18AndUnder21NationalMinimumWage { get; set; }
    public decimal? Under18NationalMinimumWage { get; set; }
    public decimal? ApprenticeMinimumWage { get; set; }
    public DateTime? CandidateDateOfBirth { get; set; }
    public ApprenticeshipTypes ApprenticeshipType { get; set; }
}

[ApiType("VacancyQualificationApiResponse")]
public class VacancyQualificationApiResponse
{
    public string? QualificationType { get; init; }
    public string? Subject { get; init; }
    public string? Grade { get; init; }
    public Weighting Weighting { get; init; }
}

public class Location
{
    public double Lat { get; set; }
    public double Lon { get; set; }
}

public class CandidateApplicationDetails
{
    public ApplicationStatus? Status { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public Guid ApplicationId { get; set; }
    public DateTime? WithdrawnDate { get; set; }
}

public enum Weighting
{
    Essential,
    Desired
}

public enum VacancyLocationType
{
    Unknown = 0,
    NonNational,
    National
}