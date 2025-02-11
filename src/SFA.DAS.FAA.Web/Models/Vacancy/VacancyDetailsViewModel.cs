using System.Globalization;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models.Vacancy
{
    public class VacancyDetailsViewModel : NhsVacancyDetailsViewModel
    {
        public string BackLinkUrl { get; set; }
        public string? Title { get; init; }
        public string? EmployerName { get; init; }
        public string? ContactOrganisationName { get; init; }
        public string? ContactName { get; init; }
        public string? ContactEmail { get; init; }
        public string? ContactPhone { get; init; }
        public string? EmployerDescription { get; init; }
        public string? EmployerWebsite { get; init; }
        public string? VacancyReference { get; init; }
        public string? VacancySummary { get; init; }
        public string? AnnualWage { get; init; }
        public string? Levels { get; init; }
        public string? WorkingPattern { get; init; }
        public string? HoursPerWeek { get; init; }
        public string? StartDate { get; init; }
        public string? PostedDate { get; init; }
        public string? ClosingDate { get; init; }
        public string ClosedDate { get; init; }
        public string? Duration { get; init; }
        public int? PositionsAvailable { get; init; }
        public Address WorkLocation { get; init; } = new();
        public List<Address> Addresses { get; init; } = [];

        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmploymentLocationOption { get; set; }
        public bool IsAnonymousEmployer { get; set; } = false;
        public string? WorkDescription { get; init; }
        public string? TrainingProviderName { get; init; }
        public List<string>? Skills { get; init; } = [];
        public string? CourseTitle { get; init; }
        public string? ThingsToConsider { get; init; }
        public string? OutcomeDescription { get; init; }
        public bool IsDisabilityConfident { get; init; } = false;
        public List<Qualification>? EssentialQualifications { get; init; } = [];
        public List<Qualification>? DesiredQualifications { get; init; } = [];
        public string? StandardPageUrl { get; init; }
        public string? CourseOverviewOfRole { get; init; }
        public List<string>? CourseCoreDuties { get; init; } = [];
        public List<string>? CourseSkills { get; init; } = [];
        public List<LevelResponse>? CourseLevels { get; init; } = [];
        public string? CourseLevelMapper { get; init; }
        public bool IsClosed { get; set; }
        public CandidateApplicationDetails? ApplicationDetails { get; set; }
        public bool ShowAccountCreatedBanner { get; set; } = false;
        public string? TrainingPlan { get; set; } //TODO
        public string? CompanyBenefits { get; set; }
        public string? WageAdditionalInformation { get; set; }
        public string? AdditionalTrainingInformation { get; set; }
        public string? GoogleMapsId { get; set; }
        public string? CandidatePostcode { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? ApplicationUrl { get; set; }
        public bool IsSavedVacancy { get; set; } = false;
        public string? ApplicationInstructions { get; set; }
        public VacancyDataSource VacancySource { get; set; }
        public string EmploymentWorkLocation => EmploymentLocationOption switch
        {
            AvailableWhere.MultipleLocations => VacancyDetailsHelperService.GetEmploymentLocationCityNames(Addresses),
            AvailableWhere.AcrossEngland => "Recruiting nationally",
            _ => VacancyDetailsHelperService.GetOneLocationCityName(WorkLocation)
        };

        public VacancyDetailsViewModel MapToViewModel(IDateTimeService dateTimeService,
            GetApprenticeshipVacancyQueryResult source, string? googleMapsId)
        {
            var addresses = new List<Address>();
            if (source.Vacancy?.Address != null) addresses.Add(source.Vacancy.Address);
            if (source.Vacancy?.OtherAddresses != null) addresses.AddRange(source.Vacancy.OtherAddresses);

            return new VacancyDetailsViewModel
            {
                Title = source.Vacancy?.Title,
                VacancyReference = source.Vacancy?.VacancyReference,
                VacancySummary = source.Vacancy?.Description,
                AnnualWage = source.Vacancy?.WageText,
                HoursPerWeek = source.Vacancy?.HoursPerWeek.ToString().GetWorkingHours(),
                Duration = source.Vacancy?.ExpectedDuration?.ToLower(),
                PositionsAvailable = source.Vacancy?.NumberOfPositions,
                WorkDescription = source.Vacancy?.LongDescription,
                ThingsToConsider = source.Vacancy?.ThingsToConsider,
                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.Vacancy.ClosingDate,
                    !string.IsNullOrEmpty(source.Vacancy?.ApplicationUrl)),
                PostedDate = source.Vacancy.PostedDate.GetPostedDate(),
                StartDate = source.Vacancy.StartDate.GetStartDate(),
                WorkLocation = source.Vacancy.Address,
                WorkingPattern = source.Vacancy?.WorkingWeek,
                TrainingProviderName = source.Vacancy?.ProviderName,
                TrainingPlan = source.Vacancy?.TrainingDescription,
                OutcomeDescription = source.Vacancy?.OutcomeDescription,
                Skills = source.Vacancy?.Skills?.ToList(),
                EmployerWebsite =
                    VacancyDetailsHelperService.FormatEmployerWebsiteUrl(source.Vacancy?.EmployerWebsiteUrl),
                EmployerDescription = source.Vacancy?.EmployerDescription,
                EmployerName = source.Vacancy?.EmployerName,
                ContactOrganisationName = string.IsNullOrWhiteSpace(source.Vacancy?.EmployerContactName)
                    ? source.Vacancy!.ProviderName
                    : source.Vacancy!.EmployerName,
                ContactName = source.Vacancy?.EmployerContactName ?? source.Vacancy?.ProviderContactName,
                ContactEmail = source.Vacancy?.EmployerContactEmail ?? source.Vacancy?.ProviderContactEmail,
                ContactPhone = source.Vacancy?.EmployerContactPhone ?? source.Vacancy?.ProviderContactPhone,
                CourseTitle = $"{source.Vacancy?.CourseTitle} (level {source.Vacancy?.CourseLevel})",
                EssentialQualifications = source.Vacancy?.Qualifications?
                    .Where(fil => fil.Weighting == Weighting.Essential).Select(l => (Qualification) l).ToList(),
                DesiredQualifications = source.Vacancy?.Qualifications?.Where(fil => fil.Weighting == Weighting.Desired)
                    .Select(l => (Qualification) l).ToList(),
                CourseSkills = source.Vacancy?.CourseSkills,
                CourseCoreDuties = source.Vacancy?.CourseCoreDuties,
                CourseOverviewOfRole = source.Vacancy?.CourseOverviewOfRole,
                StandardPageUrl = source.Vacancy?.StandardPageUrl,
                IsDisabilityConfident = source.Vacancy is {IsDisabilityConfident: true},
                CourseLevels = source.Vacancy?.Levels,
                CourseLevelMapper = int.TryParse(source.Vacancy?.CourseLevel, out _) && source.Vacancy.Levels?.Count > 0
                    ? source.Vacancy?.Levels
                        .FirstOrDefault(le => le.Code == Convert.ToInt16(source.Vacancy?.CourseLevel))?.Name
                    : string.Empty,
                ApplicationDetails = source.Vacancy?.Application,
                IsClosed = source.Vacancy?.IsClosed ?? false,
                ClosedDate =
                    $"This apprenticeship closed on {source.Vacancy?.ClosingDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture) ?? string.Empty}.",
                ApplicationUrl =
                    !string.IsNullOrEmpty(source.Vacancy?.ApplicationUrl) &&
                    !source.Vacancy.ApplicationUrl.StartsWith("http")
                        ? $"https://{source.Vacancy.ApplicationUrl}"
                        : source.Vacancy.ApplicationUrl,
                ApplicationInstructions = source.Vacancy.ApplicationInstructions,
                GoogleMapsId = googleMapsId,
                CandidatePostcode = source.Vacancy?.CandidatePostcode,
                Latitude = source.Vacancy?.Location?.Lat,
                Longitude = source.Vacancy?.Location?.Lon,
                CompanyBenefits = source.Vacancy?.CompanyBenefitsInformation,
                AdditionalTrainingInformation = source.Vacancy?.AdditionalTrainingDescription,
                WageAdditionalInformation = source.Vacancy?.WageAdditionalInformation,
                IsSavedVacancy = source.Vacancy.IsSavedVacancy,
                VacancySource = source.Vacancy.VacancySource,
                Address = source.Vacancy.Address?.ToSingleLineAbridgedAddress(),
                Addresses = addresses.OrderByCity().ToList(),
                EmploymentLocationOption = source.Vacancy?.EmploymentLocationOption,
                EmploymentLocationInformation = source.Vacancy?.EmploymentLocationInformation,
                IsAnonymousEmployer = source.Vacancy?.IsEmployerAnonymous ?? false,
            };
        }
        
    }

    public class Qualification
    {
        public string? QualificationType { get; private init; }
        public string? Subject { get; private init; }
        public string? Grade { get; private init; }
        public Weighting Weighting { get; init; }

        public static implicit operator Qualification(VacancyQualificationApiResponse source)
        {
            return new Qualification
            {
                QualificationType = source.QualificationType,
                Grade = source.Grade,
                Subject = source.Subject,
                Weighting = source.Weighting,
            };
        }
    }

    public class CandidateApplicationDetails
    {
        public ApplicationStatus? Status { get; init; }
        public string? SubmittedDate { get; init; }
        public string? WithdrawnDate { get; init; }
        public Guid ApplicationId { get; init; }

        public static implicit operator CandidateApplicationDetails?(Domain.GetApprenticeshipVacancy.CandidateApplicationDetails? source)
        {
            if (source is null) return null;

            return new CandidateApplicationDetails
            {
                Status = source.Status,
                SubmittedDate = source.SubmittedDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture),
                WithdrawnDate = source.WithdrawnDate?.ToString("d MMMM yyyy", CultureInfo.InvariantCulture),
                ApplicationId = source.ApplicationId
            };
        }
    }
}