using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Models;
public class ApprenticeshipMapData
{
    public MapPosition Position { get; set; }
    public ApprenticeshipMapJob Job { get; set; }

    public ApprenticeshipMapData MapToViewModel(IDateTimeService dateTimeService,Vacancies source)
    {
        return new ApprenticeshipMapData
        {
            Job = new ApprenticeshipMapJob
            {
                Id = source.Id,
                Title = source.Title,
                Company = source.EmployerName,
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                AddressLine3 = source.AddressLine3,
                AddressLine4 = source.AddressLine4,
                Postcode = source.Postcode,
                Distance = source.Distance,
                Apprenticeship = $"{source.CourseTitle} (level {source.CourseLevel})",
                Wage = source.WageText,
                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosingDate),
                PostedDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.PostedDate)
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
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Company { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? AddressLine4 { get; set; }
    public string? Postcode { get; set; }
    public decimal? Distance { get; set; }
    public string? Apprenticeship { get; set; }
    public string? Wage { get; set; }
    public string? ClosingDate { get; set; }
    public string? PostedDate { get; set; }
}