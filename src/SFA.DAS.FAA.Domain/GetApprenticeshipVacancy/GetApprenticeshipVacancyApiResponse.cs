using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.GetApprenticeshipVacancy
{
    public class GetApprenticeshipVacancyApiResponse
    {
        public long Id { get; init; }
        public string? Title { get; init; }
        public string? VacancyReference { get; init; }
        public string? LongDescription { get; init; }
        public string? OutcomeDescription { get; init; }
        public string? TrainingDescription { get; init; }
        public string? ThingsToConsider { get; init; }
        public IEnumerable<string>? Skills { get; init; }

        public DateTime StartDate { get; init; }
        public DateTime ClosingDate { get; set; }
        public DateTime PostedDate { get; init; }


        public string? WageAmount { get; init; }
        public int WageType { get; init; }
        public decimal? WageAmountLowerBound { get; init; }
        public decimal? WageAmountUpperBound { get; init; }
        public string? WageText { get; init; }
        public int WageUnit { get; init; }

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
        public GeoPoint Location { get; init; }
        public int NumberOfPositions { get; init; }
        public string? ProviderName { get; init; }
        public int? StandardLarsCode { get; init; }


        public VacancyLocationType VacancyLocationType { get; init; }

        public string? WorkingWeek { get; init; }
        public string? ExpectedDuration { get; init; }
        public double Score { get; init; }

        public long Ukprn { get; init; }
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
        public bool IsEmployerAnonymous { get; init; }
        public bool IsClosed { get; set; }

        public IEnumerable<VacancyQualificationApiResponse>? Qualifications { get; init; }
        public AddressApiResponse? Address { get; init; }
        public List<string> CourseSkills { get; init; }
        public List<string> CourseCoreDuties { get; init; }
        public string? CourseOverviewOfRole { get; init; }
        public string? StandardPageUrl { get; init; }
        public List<LevelResponse>? Levels { get; init; }

        public CandidateApplicationDetails? Application { get; set; }
    }

    public class VacancyQualificationApiResponse
    {
        public string? QualificationType { get; init; }
        public string? Subject { get; init; }
        public string? Grade { get; init; }
        public Weighting Weighting { get; init; }
    }

    public class AddressApiResponse
    {
        public string? AddressLine1 { get; init; }
        public string? AddressLine2 { get; init; }
        public string? AddressLine3 { get; init; }
        public string? AddressLine4 { get; init; }
        public string? Postcode { get; init; }
    }

    public class GeoPoint
    {
        public string? Postcode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class CandidateApplicationDetails
    {
        public ApplicationStatus? Status { get; set; }
        public DateTime? SubmittedDate { get; set; }
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
}
