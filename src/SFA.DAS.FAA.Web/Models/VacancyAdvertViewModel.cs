using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Models;

public class VacancyAdvertViewModel
{
    public string Title { get; private set; }
    public string EmployerName { get; private set; }
    public string CourseTitle { get;  private set; }
    public string? PostedDate { get; private set; }
    public string? StartDate { get; private set; }
    public bool ShowStartDate => VacancySource == VacancyDataSource.Raa;
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

    public static VacancyAdvertViewModel MapToViewModel(IDateTimeService dateTimeService, VacancyAdvert vacancyAdvert)
    {
        return new VacancyAdvertViewModel
        {
            ApplicationStatus = vacancyAdvert.CandidateApplicationDetails?.Status,
            ClosingDateDescription = VacancyDetailsHelperService.GetClosingDate(dateTimeService, vacancyAdvert.ClosingDate,!string.IsNullOrEmpty(vacancyAdvert.ApplicationUrl)),
            CourseTitle = vacancyAdvert.VacancySource == VacancyDataSource.Nhs ? "See more details on NHS Jobs" : $"{vacancyAdvert.CourseTitle} (level {vacancyAdvert.CourseLevel})",
            Distance = vacancyAdvert.Distance.HasValue ? Math.Round(vacancyAdvert.Distance.Value, 1) : null,
            EmployerName = vacancyAdvert.EmployerName,
            IsClosingSoon = vacancyAdvert.ClosingDate <= dateTimeService.GetDateTime().AddDays(7),
            IsDisabilityConfident = vacancyAdvert.IsDisabilityConfident,
            IsNew = vacancyAdvert.PostedDate >= dateTimeService.GetDateTime().AddDays(-7),
            IsRecruitingNationally = vacancyAdvert.IsRecruitingNationally(),
            IsSavedVacancy = vacancyAdvert.IsSavedVacancy,
            PostedDate = vacancyAdvert.PostedDate.GetPostedDate(),
            StartDate = vacancyAdvert.StartDate.ToFullDateString(),
            Title = vacancyAdvert.VacancySource == VacancyDataSource.Nhs ? $"{vacancyAdvert.Title} (from NHS Jobs)" : vacancyAdvert.Title,
            VacancyLocation = vacancyAdvert.GetLocationDescription(),
            VacancyReference = vacancyAdvert.VacancyReference,
            VacancySource = vacancyAdvert.VacancySource,
            WageText = vacancyAdvert.VacancySource == VacancyDataSource.Nhs ? VacancyDetailsHelperService.GetWageText(vacancyAdvert.WageText) : vacancyAdvert.WageText,
            WageType = (WageType)vacancyAdvert.WageType,
        };
    }
    
    public static int? CalculateDaysUntilClosing(IDateTimeService dateTimeService, DateTime? closingDate)
    {
        if (!closingDate.HasValue) return null;

        var currentDate = dateTimeService.GetDateTime();
        var timeUntilClosing = closingDate.Value.Date - currentDate;
        return (int)Math.Ceiling(timeUntilClosing.TotalDays);
    }
}