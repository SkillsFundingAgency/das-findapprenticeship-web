using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Models;

public class VacanciesViewModel
{
    public int vacancyReference { get; private set; }
    public string title { get; private set; }

    public string employerName { get; private set; }
    public string addressLine3 { get; private set; }
    public string? addressLine4 { get; private set; }
    public string vacancyPostCode { get; private set;}
    public string courseTitle { get;  private set; }
    public string wage { get; private set;  }
    public string advertClosing { get; private set; }
    public string postedDate { get; private set; }
    public string wageType { get; private set; }
    public string placeName { get; private set; }
    public double? distance { get; private set; }
    public int? daysUntilClosing { get; private set; }




    public static implicit operator VacanciesViewModel(Vacancies vacancies)
    {
        return new VacanciesViewModel
        {
            vacancyReference = vacancies.vacancyReference,
            title = vacancies.title,
            employerName = vacancies.employerName,
            addressLine3 = vacancies.addressLine3,
            addressLine4 = vacancies.addressLine4,
            vacancyPostCode = vacancies.vacancyPostCode,
            courseTitle =  vacancies.courseTitle,
            wage = vacancies.wage,
            advertClosing = FormatDate(vacancies.closingDate),
            postedDate = FormatDate(vacancies.postedDate),
            wageType = vacancies.wageType,
            placeName = vacancies.addressLine4 != null ? vacancies.addressLine4 : vacancies.addressLine3,
            distance = vacancies.distance.HasValue ? Math.Round(vacancies.distance.Value, 1) : (double?)null,
            daysUntilClosing = CalculateDaysUntilClosing(vacancies.closingDate)

        };
    }

    private static int? CalculateDaysUntilClosing(DateTime? closingDate)
    {
        if (closingDate.HasValue)
        {
            TimeSpan timeUntilClosing = closingDate.Value.Date - DateTime.Now.Date;
            return (int)Math.Ceiling(timeUntilClosing.TotalDays);
        }

        return null; 
    }

    private static string FormatDate(DateTime? date)
    {
        return date.HasValue ? date.Value.ToString("dddd dd MMMM") : null;

    }


}