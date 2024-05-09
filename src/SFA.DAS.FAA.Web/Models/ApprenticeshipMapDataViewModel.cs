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
                Title = source.Title,
                Wage = source.WageText,
                ClosingDate = VacancyDetailsHelperService.GetClosingDate(dateTimeService, source.ClosingDate),
                Apprenticeship = $"{source.CourseTitle} (level {source.CourseLevel})",
                Company = source.EmployerName
            },
            Position = new MapPosition
            {
                Lon = source.Lon,
                Lat = source.Lat
            }
        };
    }
}

public class MapPosition
{
    public double? Lat { get; set; }
    public double? Lon { get; set; }
}

public class ApprenticeshipMapJob
{
    public string Title { get; set; }
    public string Company { get; set; }
    public string Apprenticeship { get; set; }
    public string Wage { get; set; }
    public string ClosingDate { get; set; }
    // job: {
    //     title: 'Business Apprenticeship',
    //     company: 'AOF Consulting Ltd',
    //     standard: 'Business analyst (Level 4)',
    //     wage: '£25,000',
    //     closes: '30 October at 11:59pm',
    //     status: ''
    // }
}