using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;

namespace SFA.DAS.FAA.Web.Models;

public class VacanciesViewModel
{
    public string Title { get; private set; }
    public string EmployerName { get; private set; }
    public string CourseTitle { get;  private set; }
    public string? PostedDate { get; private set; }
    private WageType WageType { get; set; }
    public string VacancyLocation { get; private set; }
    public decimal? Distance { get; private set; }
    public string ClosingDateDescription { get; private set; }
    public string VacancyReference { get; private set; }
    public string WageText { get; set; }
    public ApplicationStatus? ApplicationStatus { get; set; }
    public bool IsClosingSoon { get; set; }
    public bool IsNew { get; set; }
    public bool IsDisabilityConfident { get; set; }
    public bool IsSavedVacancy { get; set; }
    public bool ShowInsertTextCssClass => WageType == WageType.CompetitiveSalary;
    public VacancyDataSource VacancySource { get; set; }

    public bool IsRecruitingNationally { get; private set; }

    public static VacanciesViewModel MapToViewModel(IDateTimeService dateTimeService, VacancyAdvert vacancyAdvert)
    {
        return new VacanciesViewModel
        {
            Title = vacancyAdvert.VacancySource == VacancyDataSource.Nhs ? $"{vacancyAdvert.Title} (from NHS Jobs)" : vacancyAdvert.Title,
            EmployerName = vacancyAdvert.EmployerName,
            CourseTitle = vacancyAdvert.VacancySource == VacancyDataSource.Nhs ? "See more details on NHS Jobs" : $"{vacancyAdvert.CourseTitle} (level {vacancyAdvert.CourseLevel})",
            ClosingDateDescription = VacancyDetailsHelperService.GetClosingDate(dateTimeService, vacancyAdvert.ClosingDate,!string.IsNullOrEmpty(vacancyAdvert.ApplicationUrl)),
            PostedDate = FormatPostDate(vacancyAdvert.PostedDate),
            WageType = (WageType)vacancyAdvert.WageType,
            IsRecruitingNationally = IsVacancyRecruitingNationally(vacancyAdvert),
            VacancyLocation = GetVacancyLocation(vacancyAdvert),
            Distance = vacancyAdvert.Distance.HasValue ? Math.Round(vacancyAdvert.Distance.Value, 1) : null,
            VacancyReference = vacancyAdvert.VacancyReference,
            WageText = vacancyAdvert.VacancySource == VacancyDataSource.Nhs ? VacancyDetailsHelperService.GetWageText(vacancyAdvert.WageText) : vacancyAdvert.WageText,
            IsClosingSoon = vacancyAdvert.ClosingDate <= dateTimeService.GetDateTime().AddDays(7),
            IsNew = vacancyAdvert.PostedDate >= dateTimeService.GetDateTime().AddDays(-7),
            IsDisabilityConfident = vacancyAdvert.IsDisabilityConfident,
            ApplicationStatus = vacancyAdvert.CandidateApplicationDetails?.Status,
            IsSavedVacancy = vacancyAdvert.IsSavedVacancy,
            VacancySource = vacancyAdvert.VacancySource,
        };
    }

    private static bool IsVacancyRecruitingNationally(VacancyAdvert vacancyAdvert) =>
        string.IsNullOrWhiteSpace(vacancyAdvert.Postcode) &&
        !string.IsNullOrWhiteSpace(vacancyAdvert.EmploymentLocationInformation);

    private static string GetVacancyLocation(VacancyAdvert vacancyAdvert)
    {
        if (IsVacancyRecruitingNationally(vacancyAdvert))
        {
            return "Recruiting nationally";
        }

        List<string?> lines = [
            vacancyAdvert.AddressLine4,
            vacancyAdvert.AddressLine3,
            vacancyAdvert.AddressLine2,
            vacancyAdvert.AddressLine1,
        ];

        var city = lines.FirstOrDefault(x => !string.IsNullOrEmpty(x));
        var location = string.IsNullOrEmpty(city)
            ? $"{vacancyAdvert.Postcode}"
            : $"{city} ({vacancyAdvert.Postcode})";
        
        return vacancyAdvert.OtherAddresses switch
        {
            { Count: 1 } => $"{location} and 1 other location",
            { Count: >1 } => $"{location} and {vacancyAdvert.OtherAddresses.Count} other locations",
            _ => location,
        };
    }

    public static int? CalculateDaysUntilClosing(IDateTimeService dateTimeService, DateTime? closingDate)
    {
        if (!closingDate.HasValue) return null;

        var currentDate = dateTimeService.GetDateTime();
        var timeUntilClosing = closingDate.Value.Date - currentDate;
        return (int)Math.Ceiling(timeUntilClosing.TotalDays);
    }

    private static string? FormatPostDate(DateTime? date)
    {
        return date?.ToString("d MMMM", CultureInfo.InvariantCulture);
    }
}