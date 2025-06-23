using SFA.DAS.FAA.Domain.Models;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Domain.Extensions;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.Services
{
    public static class VacancyDetailsHelperService
    {
        private const decimal NhsLowerWageAmountLimit = 100.00M;
        private const decimal NhsUpperWageAmountLimit = 5000.00M;
        
        public static string GetWorkingHours(this string? duration)
        {
            if (!decimal.TryParse(duration, out var workHours)) return "0 hours a week";

            var integerPart = (int) workHours;
            var decimalPart = (double) (workHours - integerPart);
            var minutes = Convert.ToInt32(decimalPart * 60);

            return minutes > 0 ? $"{integerPart} hours {minutes} minutes a week" : $"{integerPart} hours a week";
        }

        public static string GetClosingDate(IDateTimeService dateTimeService, DateTime closingDate, bool isExternalVacancy = false)
        {
            var timeSuffix = isExternalVacancy ? string.Empty : " at 11:59pm";
            var daysToExpiry = GetDaysUntilExpiry(dateTimeService, closingDate);

            return daysToExpiry switch
            {
                < 0 => $"Closed on {closingDate.ToGdsDateStringWithDayOfWeek()}",
                0 => $"Closes today{timeSuffix}",
                1 => $"Closes tomorrow ({closingDate.ToGdsDateStringWithDayOfWeek()}{timeSuffix})",
                <= 31 => $"Closes in {daysToExpiry} days ({closingDate.ToGdsDateStringWithDayOfWeek()}{timeSuffix})",
                _ => $"Closes on {closingDate.ToGdsDateStringWithDayOfWeek()}"
            };
        }
        
        public static string GetClosingDate(IDateTimeService dateTimeService, DateTime closingDate, DateTime? closedDate, bool isExternalVacancy = false)
        {
            return closedDate.HasValue
                ? $"Closed on {closedDate?.ToGdsDateStringWithDayOfWeek()}"
                : GetClosingDate(dateTimeService, closingDate, isExternalVacancy);    
        }

        public static string GetClosedDate(DateTime? closedDate)
        {
            return closedDate.HasValue
                ? $"closed at {closedDate?.ToGdsDateStringWithTime()}"
                : string.Empty;
        }

        public static int GetDaysUntilExpiry(IDateTimeService dateTimeService, DateTime closingDate)
        {
            var timeUntilClosing = closingDate.Date - dateTimeService.GetDateTime();
            return (int)Math.Ceiling(timeUntilClosing.TotalDays);
        }

        public static string? FormatEmployerWebsiteUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return url;

            var websiteUrlRegex = new Regex(@"^(http?|https):\/\/[^\s\/$.?#].[^\s]*$", RegexOptions.None, TimeSpan.FromSeconds(3));

            return !websiteUrlRegex.IsMatch(url) ? $"http://{url}" : url;
        }

        public static string GetPostedDate(this DateTime postedDate)
        {
            return $"Posted on {postedDate.ToGdsDateString()}";
        }

        public static string GetSearchResultsPostedDate(this DateTime postedDate)
        {
            return $"Posted {postedDate.ToGdsDateString()}";
        }

        public static string ToFullDateString(this DateTime datetime)
        {
            return datetime.ToString("d MMMM yyyy", CultureInfo.InvariantCulture);
        }

        public static string GetMapsPostedDate(this DateTime postedDate)
        {
            return $"Posted on {postedDate.ToGdsDateString()}";
        }

        public static string GetNhsWageText(string wageAmountText)
        {
            if (string.IsNullOrEmpty(wageAmountText)) return $"{wageAmountText}";

            var encodedBytes = Encoding.UTF8.GetBytes(wageAmountText);
            var decoded = Encoding.UTF8.GetString(encodedBytes);

            var poundRemovedStr = decoded
                .Replace("�", string.Empty) // Application env & Pipeline doesn't recognise the Pound Sign
                .Replace("£", string.Empty) 
                .Replace("\u00A3", string.Empty) // Unicode for Pound Sign
                .Replace("u+00A3", string.Empty);

            var wageTextRegex = new Regex(@"\d+\.\d{2}", RegexOptions.None, TimeSpan.FromSeconds(3));
            var matches = wageTextRegex.Matches(wageAmountText);

            if (matches.Count == 2)
            {
                var lowerBound = decimal.Parse(matches[0].Value, CultureInfo.InvariantCulture);
                var upperBound = decimal.Parse(matches[1].Value, CultureInfo.InvariantCulture);

                var lowerBoundText = lowerBound % 1 == 0
                    ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##}", lowerBound)
                    : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00}", lowerBound);
                
                var upperBoundText = upperBound % 1 == 0
                    ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##}", upperBound)
                    : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00}", upperBound);

                return upperBound switch
                {
                    > NhsUpperWageAmountLimit => $"{lowerBoundText} to {upperBoundText} a year",
                    < NhsLowerWageAmountLimit => $"{lowerBoundText} to {upperBoundText} an hour",
                    _ => $"{lowerBoundText} to {upperBoundText}",
                };
            }

            if (!decimal.TryParse(poundRemovedStr, out var wageAmount)) return $"{wageAmountText}";

            return wageAmount switch
            {
                < NhsLowerWageAmountLimit => wageAmount % 1 == 0 ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##} an hour", wageAmount) : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00} an hour", wageAmount),
                > NhsUpperWageAmountLimit => wageAmount % 1 == 0 ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##} a year", wageAmount) : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00} a year", wageAmount),
                _ => wageAmountText
            };
         }

        public static string GetVacancyAdvertWageText(IVacancyAdvert vacancyAdvert, DateTime? candidateDateOfBirth = null, bool detailsPage = false)
        {
            switch ((WageType)vacancyAdvert.WageType)
            {
                case WageType.Unknown:
                case WageType.FixedWage:
                case WageType.CompetitiveSalary:
                    return vacancyAdvert.WageText;
                case WageType.NationalMinimumWageForApprentices:
                    return vacancyAdvert.ApprenticeMinimumWage.ToDisplayWage(detailsPage ? "for your first year, then could increase depending on your age" : "a year") ?? vacancyAdvert.WageText;
            }

            if (string.IsNullOrEmpty(vacancyAdvert.WageText))
            {
                return vacancyAdvert.WageText;
            }
            
            if(!candidateDateOfBirth.HasValue)
                return !detailsPage ? vacancyAdvert.WageText : vacancyAdvert.WageText.Replace(" a year",", depending on your age");
            
            var ageAtStart = GetCandidatesAgeAtStartDateOfVacancy(candidateDateOfBirth.Value, vacancyAdvert.StartDate);
            return ageAtStart switch
            {
                < 18 => vacancyAdvert.Under18NationalMinimumWage.ToDisplayWage() ?? vacancyAdvert.WageText,
                < 21 => vacancyAdvert.Between18AndUnder21NationalMinimumWage.ToDisplayWage() ?? vacancyAdvert.WageText,
                < 25 => vacancyAdvert.Between21AndUnder25NationalMinimumWage.ToDisplayWage() ?? vacancyAdvert.WageText,
                >= 25 => vacancyAdvert.Over25NationalMinimumWage.ToDisplayWage() ?? vacancyAdvert.WageText
            };
        }

        public static string GetVacancyAdvertDetailWageDescriptionText(WageType wageType, DateTime startDate, DateTime? candidateDateOfBirth = null)
        {
            switch (wageType)
            {
                case WageType.Unknown:
                case WageType.FixedWage:
                    return "";
                case WageType.CompetitiveSalary:
                    return "Competitive wage offered";
                case WageType.NationalMinimumWageForApprentices:
                    return "National Minimum Wage rate for apprentices";
            }

            if (!candidateDateOfBirth.HasValue)
            {
                return "National Minimum Wage";
            }
            
            var candidateAge = GetCandidatesAgeAtStartDateOfVacancy(candidateDateOfBirth.Value, startDate);

            if (candidateAge <= 18)
            {
                return "National Minimum Wage for an under 18 year old";
            }
            
            return $"National Minimum Wage for a {candidateAge} year old";
        }

        public static int GetCandidatesAgeAtStartDateOfVacancy(DateTime candidateDateOfBirth, DateTime startDate)
        {
            var ageAtStart = (int.Parse(startDate.ToString("yyyyMMdd")) -
                              int.Parse(candidateDateOfBirth.ToString("yyyyMMdd"))) / 10000;
            return ageAtStart;
        }
        
        public static string GetEmploymentLocationCityNames(List<Address> addresses)
        {
            var cityNames = addresses
                .Select(address => address.GetLastNonEmptyField())
                .OfType<string>()
                .Distinct()
                .OrderBy(city => city)
                .ToList();

            return cityNames.Count == 1 && addresses.Count > 1
                ? $"{cityNames.First()} ({addresses.Count} available locations)"
                : string.Join(", ", cityNames);
        }

        public static string? GetOneLocationCityName(Address? address)
        {
            if (address is null)
            {
                return null;
            }
            var city = address.GetLastNonEmptyField();
            return string.IsNullOrWhiteSpace(city) ? address.Postcode! : $"{city} ({address.Postcode})";
        }
    }
}