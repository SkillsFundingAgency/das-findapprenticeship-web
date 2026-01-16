using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

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
    public string VacancyId { get; set; }
    public string WageText { get; set; }
    public ApplicationStatus? ApplicationStatus { get; set; }
    public bool IsClosingSoon { get; set; }
    public bool IsNew { get; set; }
    public bool IsDisabilityConfident { get; set; }
    public bool IsSavedVacancy { get; set; }
    public bool ShowInsertTextCssClass => WageType == WageType.CompetitiveSalary;
    public VacancyDataSource VacancySource { get; set; }

    public bool IsRecruitingNationally { get; private set; }
    public ApprenticeshipTypes ApprenticeshipType { get; init; }

    public static VacancyAdvertViewModel MapToViewModel(IDateTimeService dateTimeService, VacancyAdvert vacancyAdvert, DateTime? candidateDateOfBirth)
    {
        return new VacancyAdvertViewModel
        {
            ApplicationStatus = vacancyAdvert.CandidateApplicationDetails?.Status,
            ApprenticeshipType = vacancyAdvert.ApprenticeshipType,
            ClosingDateDescription = VacancyDetailsHelperService.GetClosingDate(dateTimeService, vacancyAdvert.ClosingDate,!string.IsNullOrEmpty(vacancyAdvert.ApplicationUrl)),
            CourseTitle = GetCourseTitle(vacancyAdvert),
            Distance = vacancyAdvert.Distance.HasValue ? Math.Round(vacancyAdvert.Distance.Value, 1) : null,
            EmployerName = vacancyAdvert.EmployerName,
            IsClosingSoon = vacancyAdvert.ClosingDate <= dateTimeService.GetDateTime().AddDays(7),
            IsDisabilityConfident = vacancyAdvert.IsDisabilityConfident,
            IsNew = vacancyAdvert.PostedDate >= dateTimeService.GetDateTime().AddDays(-7),
            IsRecruitingNationally = vacancyAdvert.IsRecruitingNationally(),
            IsSavedVacancy = vacancyAdvert.IsSavedVacancy,
            PostedDate = vacancyAdvert.PostedDate.GetSearchResultsPostedDate(),
            StartDate = vacancyAdvert.StartDate.ToFullDateString(),
            Title = GetTitle(vacancyAdvert),
            VacancyLocation = vacancyAdvert.GetLocationDescription(),
            VacancyReference = vacancyAdvert.VacancyReference,
            VacancyId = vacancyAdvert.Id,
            VacancySource = vacancyAdvert.VacancySource,
            WageText = vacancyAdvert.VacancySource == VacancyDataSource.Raa 
                ? VacancyDetailsHelperService.GetVacancyAdvertWageText(vacancyAdvert, candidateDateOfBirth)
                : VacancyDetailsHelperService.GetExternalVacancyAdvertWageText(vacancyAdvert.WageText),
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


    private static string GetTitle(VacancyAdvert advert)
    {
        return advert.VacancySource switch
        {
            VacancyDataSource.Raa => advert.Title,
            VacancyDataSource.Nhs => $"{advert.Title} (from NHS Jobs)",
            VacancyDataSource.Csj => $"{advert.Title} (from Civil Service Jobs)",
            _ => string.Empty
        };
    }

    private static string GetCourseTitle(VacancyAdvert advert)
    {
        return advert.VacancySource switch
        {
            VacancyDataSource.Raa => VacancyDetailsHelperService.GetCourseTitle(advert.CourseTitle, advert.CourseLevel),
            VacancyDataSource.Nhs => "See more details on NHS Jobs",
            VacancyDataSource.Csj => "See more details on Civil Service Jobs",
            _ => string.Empty
        };
    }
}