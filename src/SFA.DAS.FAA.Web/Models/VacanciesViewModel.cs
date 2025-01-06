using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;

namespace SFA.DAS.FAA.Web.Models;

public class VacanciesViewModel
{
    public string Id { get; private set; }
    public string Title { get; private set; }
    public string EmployerName { get; private set; }
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    public string VacancyPostCode { get; private set;}
    public string CourseTitle { get;  private set; }
    public string? PostedDate { get; private set; }
    private WageType WageType { get; set; }
    public string VacancyLocation { get; private set; }
    public decimal? Distance { get; private set; }
    public string ClosingDateDescription { get; private set; }
    public string ApprenticeshipLevel { get; private set; }
    public int CourseId { get; set; }
    public string CourseLevel { get; set; }
    public string VacancyReference { get; private set; }
    public string WageText { get; set; }
    public ApplicationStatus? ApplicationStatus { get; set; }
    public bool IsClosingSoon { get; set; }
    public bool IsNew { get; set; }
    public bool IsDisabilityConfident { get; set; }
    public bool IsSavedVacancy { get; set; } = false;
    public bool ShowInsertTextCssClass => WageType == WageType.CompetitiveSalary;
    public VacancyDataSource VacancySource { get; set; }

    public VacanciesViewModel MapToViewModel(IDateTimeService dateTimeService, Vacancies vacancies)
    {
        return new VacanciesViewModel
        {
            Id = vacancies.Id,
            Title = vacancies.VacancySource == VacancyDataSource.Nhs ? $"{vacancies.Title} (from NHS Jobs)" : vacancies.Title,
            EmployerName = vacancies.EmployerName,
            AddressLine1 = vacancies.AddressLine1,
            AddressLine2 = vacancies.AddressLine2,
            AddressLine3 = vacancies.AddressLine3,
            AddressLine4 = vacancies.AddressLine4,
            VacancyPostCode = vacancies.Postcode,
            CourseTitle = vacancies.VacancySource == VacancyDataSource.Nhs ? "See more details on NHS Jobs" : $"{vacancies.CourseTitle} (level {vacancies.CourseLevel})",
            ClosingDateDescription = VacancyDetailsHelperService.GetClosingDate(dateTimeService, vacancies.ClosingDate,!string.IsNullOrEmpty(vacancies.ApplicationUrl)),
            PostedDate = FormatPostDate(vacancies.PostedDate),
            WageType = (WageType)vacancies.WageType,
            VacancyLocation = !string.IsNullOrEmpty(vacancies.AddressLine4) ? $"{vacancies.AddressLine4}, {vacancies.Postcode}" :
                !string.IsNullOrEmpty(vacancies.AddressLine3) ? $"{vacancies.AddressLine3}, {vacancies.Postcode}" :
                !string.IsNullOrEmpty(vacancies.AddressLine2) ? $"{vacancies.AddressLine2}, {vacancies.Postcode}" :
                !string.IsNullOrEmpty(vacancies.AddressLine1) ? $"{vacancies.AddressLine1}, {vacancies.Postcode}" :
                vacancies.Postcode,
            Distance = vacancies.Distance.HasValue ? Math.Round(vacancies.Distance.Value, 1) : null,
            ApprenticeshipLevel = vacancies.ApprenticeshipLevel,
            CourseId = vacancies.CourseId,
            CourseLevel = vacancies.CourseLevel,
            VacancyReference = vacancies.VacancyReference,
            WageText = vacancies.VacancySource == VacancyDataSource.Nhs ? VacancyDetailsHelperService.GetWageText(vacancies.WageText) : vacancies.WageText,
            IsClosingSoon = vacancies.ClosingDate <= dateTimeService.GetDateTime().AddDays(7),
            IsNew = vacancies.PostedDate >= dateTimeService.GetDateTime().AddDays(-7),
            IsDisabilityConfident = vacancies.IsDisabilityConfident,
            ApplicationStatus = vacancies.CandidateApplicationDetails?.Status,
            IsSavedVacancy = vacancies.IsSavedVacancy,
            VacancySource = vacancies.VacancySource,
        };
    }

    

    public static int? CalculateDaysUntilClosing(IDateTimeService dateTimeService, DateTime? closingDate)
    {
        if (!closingDate.HasValue) return null;

        var currentDate = dateTimeService.GetDateTime();
        var timeUntilClosing = closingDate.Value.Date - currentDate;
        return (int)Math.Ceiling(timeUntilClosing.TotalDays);
    }

    private static string? FormatCloseDate(DateTime? date)
    {
        return date?.ToString("dddd d MMMM", CultureInfo.InvariantCulture);
    }

    private static string? FormatPostDate(DateTime? date)
    {
        return date?.ToString("d MMMM", CultureInfo.InvariantCulture);
    }
}