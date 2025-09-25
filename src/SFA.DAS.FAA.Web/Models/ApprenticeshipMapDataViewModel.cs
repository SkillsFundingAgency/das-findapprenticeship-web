using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Models;

public class ApprenticeshipMapData
{
    public MapPosition Position { get; set; }
    public ApprenticeshipMapJob Job { get; set; }

    public static ApprenticeshipMapData MapToViewModel(IDateTimeService dateTimeService, VacancyAdvert source, DateTime? candidateDateOfBirth)
    {
        return new ApprenticeshipMapData
        {
            Job = new ApprenticeshipMapJob
            {
                Id = source.Id,
                Title = source.Title,
                Company = source.EmployerName,
                VacancyLocation = source.GetLocationDescription(),
                Distance = source.Distance.HasValue ? Math.Round(source.Distance.Value, 1) : null,
                Apprenticeship = $"{source.CourseTitle} (level {source.CourseLevel})",
                Wage = VacancyDetailsHelperService.GetVacancyAdvertWageText(source, candidateDateOfBirth),
                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosingDate, !string.IsNullOrEmpty(source.ApplicationUrl)),
                PostedDate = source.PostedDate.GetMapsPostedDate(),
                ApplicationStatus =  source.CandidateApplicationDetails?.Status?.GetLabel(),
                ApplicationStatusCssClass =  source.CandidateApplicationDetails?.Status?.GetCssClass(),
                IsClosingSoon = source.ClosingDate <= dateTimeService.GetDateTime().AddDays(7),
                IsNew = source.PostedDate >= dateTimeService.GetDateTime().AddDays(-7),
                IsFoundation = source.ApprenticeshipType == ApprenticeshipTypes.Foundation,
            },
            Position = new MapPosition
            {
                Lng = source.Lon,
                Lat = source.Lat
            }
        };
    }
}

public class MapPosition
{
    public double? Lat { get; set; }
    public double? Lng { get; set; }
}

public class ApprenticeshipMapJob
{
    public string Id { get; set; }
    public string? Title { get; set; }
    public string? Company { get; set; }
    public string? VacancyLocation { get; set; }
    public decimal? Distance { get; set; }
    public string? Apprenticeship { get; set; }
    public string? Wage { get; set; }
    public string? ClosingDate { get; set; }
    public string? PostedDate { get; set; }
    public string? ApplicationStatus { get; set; }
    public string? ApplicationStatusCssClass { get; set; }
    public bool IsClosingSoon { get; set; }
    public bool IsNew { get; set; }
    public bool IsFoundation { get; set; }
}