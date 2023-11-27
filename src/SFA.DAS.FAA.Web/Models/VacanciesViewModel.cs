using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Services;

namespace SFA.DAS.FAA.Web.Models;

public class VacanciesViewModel
{
    private readonly IDateTimeService dateTimeService;

    public VacanciesViewModel(IDateTimeService dateTimeService)
    {
        this.dateTimeService = dateTimeService;
    }

    public int VacancyReference { get; private set; }
    public string Title { get; private set; }

    public string EmployerName { get; private set; }
    public string AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    public string VacancyPostCode { get; private set;}
    public string CourseTitle { get;  private set; }
    public double? WageAmount { get; private set;  }
    public string AdvertClosing { get; private set; }
    public string PostedDate { get; private set; }
    public string WageType { get; private set; }
    public string VacancyLocation { get; private set; }
    public double? Distance { get; private set; }
    public int? DaysUntilClosing { get; private set; }
    public string Wage { get; private set; }




    public static implicit operator VacanciesViewModel(Vacancies vacancies)
    {
        IDateTimeService dateTimeService = new DateTimeService();
        return new VacanciesViewModel (dateTimeService)
        {
            VacancyReference = vacancies.VacancyReference,
            Title = vacancies.Title,
            EmployerName = vacancies.EmployerName,
            AddressLine1 = vacancies.Address.AddressLine1,
            AddressLine2 = vacancies.Address.AddressLine2,
            AddressLine3 = vacancies.Address.AddressLine3,
            AddressLine4 = vacancies.Address.AddressLine4,
            VacancyPostCode = vacancies.Address.Postcode,
            CourseTitle =  vacancies.Course.Title,
            WageAmount = vacancies.Wage.WageAmount,
            AdvertClosing = FormatCloseDate(vacancies.ClosingDate),
            PostedDate = FormatPostDate(vacancies.PostedDate),
            WageType = vacancies.Wage.WageType,
            VacancyLocation = vacancies.Address?.AddressLine4 ??
                        vacancies.Address?.AddressLine3 ??
                        vacancies.Address?.AddressLine2 ??
                        vacancies.Address?.AddressLine1 ?? string.Empty,
            Distance = vacancies.Distance.HasValue ? Math.Round(vacancies.Distance.Value, 1) : (double?)null,
            DaysUntilClosing = CalculateDaysUntilClosing(dateTimeService, vacancies.ClosingDate),
            Wage = GetWage(vacancies.Wage.WageType, vacancies.Wage.WageAmount)

        };
    }

    public static int? CalculateDaysUntilClosing(IDateTimeService dateTimeService, DateTime? closingDate)
    {
        if (closingDate.HasValue)
        {
            DateTime currentDate = dateTimeService.GetDateTime();
            TimeSpan timeUntilClosing = closingDate.Value.Date - currentDate;
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

    private static string GetWage(string wageType, double? wageAmount)
    {
        switch (wageType)
        {
            case "Custom":
                return wageAmount.HasValue ? wageAmount.Value.ToString("C") : "DefaultCurrency"; 

            case "ApprenticeshipMinimum":
                return "£10,158.72";

            default:
                 return "£10,982.40 to £21,673.60";

        }
    }

}