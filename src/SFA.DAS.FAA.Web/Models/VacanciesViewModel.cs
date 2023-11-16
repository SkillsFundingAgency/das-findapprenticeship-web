using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Web.Models;

public class VacanciesViewModel
{
    public int vacancyReference { get; private set; }
    public string title { get; private set; }

    public string employerName { get; private set; }
    public string addressLine1 { get; private set; }
    public string? addressLine2 { get; private set; }
    public string addressLine3 { get; private set; }
    public string? addressLine4 { get; private set; }
    public string vacancyPostCode { get; private set;}
    public string courseTitle { get;  private set; }
    public double wageAmount { get; private set;  }
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
            addressLine1 = vacancies.address.addressLine1,
            addressLine2 = vacancies.address.addressLine2,
            addressLine3 = vacancies.address.addressLine3,
            addressLine4 = vacancies.address.addressLine4,
            vacancyPostCode = vacancies.address.postcode,
            courseTitle =  vacancies.course.title,
            wageAmount = vacancies.wage.wageAmount,
            advertClosing = FormatCloseDate(vacancies.closingDate),
            postedDate = FormatPostDate(vacancies.postedDate),
            wageType = vacancies.wage.wageType,
            placeName = vacancies.address?.addressLine4 ??
                        vacancies.address?.addressLine3 ??
                        vacancies.address?.addressLine2 ??
                        vacancies.address?.addressLine1 ?? string.Empty,
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

    private static string FormatCloseDate(DateTime? date)
    {
        return date.HasValue ? date.Value.ToString("dddd dd MMMM") : null;

    }

    private static string FormatPostDate(DateTime? date)
    {
        return date.HasValue ? date.Value.ToString("dd MMMM") : null;

    }

}