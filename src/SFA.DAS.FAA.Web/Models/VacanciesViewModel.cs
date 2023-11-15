using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Models;

public class VacanciesViewModel
{
    public int vacancyId { get; private set; }
    public string title { get; private set; }

    public string employerName { get; private set; }
    public string vacancyLocation { get; private set; }
    public string vacancyPostCode { get; private set;}
    public string courseTitle { get;  private set; }
    public string wage { get; private set;  }
    public string level { get; private set; }
    public DateTime advertClosing { get; private set; }
    public DateTime postedDate { get; private set; }

    

    public static implicit operator VacanciesViewModel(Vacancies vacancies)
    {
        return new VacanciesViewModel
        {
            vacancyId = vacancies.vacancyId,
            title = vacancies.title,
            employerName = vacancies.employerName,
            vacancyLocation = vacancies.vacancyLocation,
            vacancyPostCode = vacancies.vacancyPostCode,
            courseTitle =  vacancies.courseTitle,
            level = vacancies.Level,
            wage = vacancies.wage,
            advertClosing = vacancies.advertClosing,
            postedDate = vacancies.postedDate,
        };
    }


}