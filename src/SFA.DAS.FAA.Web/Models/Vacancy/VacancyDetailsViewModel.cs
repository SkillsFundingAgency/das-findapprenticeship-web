using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Domain.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models.Vacancy
{
    public class VacancyDetailsViewModel
    {
        public string? Title { get; init; }

        public string? EmployerName { get; init; }
        public string? EmployerEmail { get; init; }
        public string? EmployerPhone { get; init; }
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
        public string? ClosingDate {get; init; }
        public string? Duration { get; init; }
        public int PositionsAvailable { get; init; }

        public Address WorkLocation { get; init; } = new();
        public string? WorkDescription { get; init; }

        public string? TrainingProviderName { get; init; }
        public string? TrainingDescription { get; init; }
        public List<string> Skills { get; init; } = new();


        public bool IsDisabilityConfident { get; init; }

        public List<Qualification>? EssentialQualifications { get; init; } = new();
        public List<Qualification>? DesiredQualifications { get; init; } = new();

        public VacancyDetailsViewModel MapToViewModel(IDateTimeService dateTimeService, GetApprenticeshipVacancyQueryResult source)
        {
            return new VacancyDetailsViewModel
            {
                Title = source.Vacancy.Title,
                VacancyReference = source.Vacancy.VacancyReference,

                VacancySummary = source.Vacancy.Description,

                AnnualWage = source.Vacancy.WageText,
                HoursPerWeek = source.Vacancy.HoursPerWeek.ToString().GetWorkingHours(),

                Duration = source.Vacancy.ExpectedDuration,
                PositionsAvailable = source.Vacancy.NumberOfPositions,

                WorkDescription = source.Vacancy.TrainingDescription,

                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.Vacancy.ClosingDate),
                PostedDate = source.Vacancy.PostedDate.GetPostedDate(),
                StartDate = source.Vacancy.StartDate.GetStartDate(),

                WorkLocation = source.Vacancy.Address,

                TrainingProviderName = source.Vacancy.ProviderName,
                TrainingDescription = source.Vacancy.TrainingDescription,

                Skills = source.Vacancy.Skills.ToList(),

                EmployerWebsite = source.Vacancy.EmployerWebsiteUrl,
                EmployerDescription = source.Vacancy.EmployerDescription,

                EmployerName = source.Vacancy.EmployerName,
                EmployerEmail = source.Vacancy.EmployerContactEmail,
                EmployerPhone = source.Vacancy.EmployerContactPhone,

                EssentialQualifications = source.Vacancy.Qualifications.Where(fil => fil.Weighting == Weighting.Essential).Select(l => (Qualification)l).ToList(),
                DesiredQualifications = source.Vacancy.Qualifications.Where(fil => fil.Weighting == Weighting.Desired).Select(l => (Qualification)l).ToList(),
            };
        }
    }

    public class Address
    {
        public string? AddressLine1 { get; init; }
        public string? AddressLine2 { get; init; }
        public string? AddressLine3 { get; init; }
        public string? AddressLine4 { get; init; }
        public string? Postcode { get; init; }

        public static implicit operator Address(AddressApiResponse source)
        {
            return new Address
            {
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode,
            };
        }
    }

    public class Qualification
    {
        public string? QualificationType { get; init; }
        public string? Subject { get; init; }
        public string? Grade { get; init; }
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
}
