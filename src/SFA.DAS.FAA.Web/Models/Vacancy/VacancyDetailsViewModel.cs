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
        public Address WorkLocation { get; init; } = Domain.Models.Address.Empty;
        public List<Address> Addresses { get; init; } = [];
        public string? EmploymentLocationInformation { get; set; }
        public AvailableWhere? EmployerLocationOption { get; set; }
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
        public MapLocation[] MapLocations { get; init; } = [];
        
        public string? EmploymentWorkLocation => EmployerLocationOption switch
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

            var mapLocations = addresses
                .Where(x => x is { Latitude: not null and not 0, Longitude: not null and not 0 })
                .Select(x =>
                    new MapLocation(
                        source.Vacancy!.IsEmployerAnonymous
                            ? x.ToSingleLineAnonymousFullAddress()
                            : x.ToSingleLineFullAddress(),
                        x.Latitude!.Value.ToGeoDecimalWithMetreAccuracy(),
                        x.Longitude!.Value.ToGeoDecimalWithMetreAccuracy()))
                .OrderBy(x => x.Address)
                .ToArray();

            return new VacancyDetailsViewModel
            {
                AdditionalTrainingInformation = source.Vacancy?.AdditionalTrainingDescription,
                Address = source.Vacancy.Address?.ToSingleLineAbridgedAddress(),
                Addresses = addresses.OrderByCity().ToList(),
                AnnualWage = source.Vacancy?.WageText,
                ApplicationDetails = source.Vacancy?.Application,
                ApplicationInstructions = source.Vacancy.ApplicationInstructions,
                ApplicationUrl = !string.IsNullOrEmpty(source.Vacancy?.ApplicationUrl) && !source.Vacancy.ApplicationUrl.StartsWith("http") ? $"https://{source.Vacancy.ApplicationUrl}" : source.Vacancy.ApplicationUrl,
                CandidatePostcode = source.Vacancy?.CandidatePostcode,
                ClosedDate = $"This apprenticeship closed on {source.Vacancy?.ClosingDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture) ?? string.Empty}.",
                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.Vacancy.ClosingDate, !string.IsNullOrEmpty(source.Vacancy?.ApplicationUrl)),
                CompanyBenefits = source.Vacancy?.CompanyBenefitsInformation,
                ContactEmail = source.Vacancy?.EmployerContactEmail ?? source.Vacancy?.ProviderContactEmail,
                ContactName = source.Vacancy?.EmployerContactName ?? source.Vacancy?.ProviderContactName,
                ContactOrganisationName = string.IsNullOrWhiteSpace(source.Vacancy?.EmployerContactName) ? source.Vacancy!.ProviderName : source.Vacancy!.EmployerName,
                ContactPhone = source.Vacancy?.EmployerContactPhone ?? source.Vacancy?.ProviderContactPhone,
                CourseCoreDuties = source.Vacancy?.CourseCoreDuties,
                CourseLevelMapper = int.TryParse(source.Vacancy?.CourseLevel, out _) && source.Vacancy.Levels?.Count > 0 ? source.Vacancy?.Levels.FirstOrDefault(le => le.Code == Convert.ToInt16(source.Vacancy?.CourseLevel))?.Name : string.Empty,
                CourseLevels = source.Vacancy?.Levels,
                CourseOverviewOfRole = source.Vacancy?.CourseOverviewOfRole,
                CourseSkills = source.Vacancy?.CourseSkills,
                CourseTitle = $"{source.Vacancy?.CourseTitle} (level {source.Vacancy?.CourseLevel})",
                DesiredQualifications = source.Vacancy?.Qualifications?.Where(fil => fil.Weighting == Weighting.Desired).Select(l => (Qualification) l).ToList(),
                Duration = source.Vacancy?.ExpectedDuration?.ToLower(),
                EmployerDescription = source.Vacancy?.EmployerDescription,
                EmployerLocationOption = source.Vacancy?.EmployerLocationOption,
                EmployerName = source.Vacancy?.EmployerName,
                EmployerWebsite = VacancyDetailsHelperService.FormatEmployerWebsiteUrl(source.Vacancy?.EmployerWebsiteUrl),
                EmploymentLocationInformation = source.Vacancy?.EmploymentLocationInformation,
                EssentialQualifications = source.Vacancy?.Qualifications?.Where(fil => fil.Weighting == Weighting.Essential).Select(l => (Qualification) l).ToList(),
                GoogleMapsId = googleMapsId,
                HoursPerWeek = source.Vacancy?.HoursPerWeek.ToString().GetWorkingHours(),
                IsAnonymousEmployer = source.Vacancy?.IsEmployerAnonymous ?? false,
                IsClosed = source.Vacancy?.IsClosed ?? false,
                IsDisabilityConfident = source.Vacancy is {IsDisabilityConfident: true},
                IsSavedVacancy = source.Vacancy.IsSavedVacancy,
                Latitude = source.Vacancy?.Location?.Lat,
                Longitude = source.Vacancy?.Location?.Lon,
                MapLocations = mapLocations,
                OutcomeDescription = source.Vacancy?.OutcomeDescription,
                PositionsAvailable = source.Vacancy?.NumberOfPositions,
                PostedDate = source.Vacancy.PostedDate.GetPostedDate(),
                Skills = source.Vacancy?.Skills?.ToList(),
                StandardPageUrl = source.Vacancy?.StandardPageUrl,
                StartDate = source.Vacancy.StartDate.GetStartDate(),
                ThingsToConsider = source.Vacancy?.ThingsToConsider,
                Title = source.Vacancy?.Title,
                TrainingPlan = source.Vacancy?.TrainingDescription,
                TrainingProviderName = source.Vacancy?.ProviderName,
                VacancyReference = source.Vacancy?.VacancyReference,
                VacancySource = source.Vacancy.VacancySource,
                VacancySummary = source.Vacancy?.Description,
                WageAdditionalInformation = source.Vacancy?.WageAdditionalInformation,
                WorkDescription = source.Vacancy?.LongDescription,
                WorkLocation = source.Vacancy.Address,
                WorkingPattern = source.Vacancy?.WorkingWeek,
            };
        }
    }

    public record MapLocation(string Address, decimal Lat, decimal Lon);

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