using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;
using System.Text;

namespace SFA.DAS.FAA.Web.Services
{
    public static partial class VacancyDetailsHelperService
    {
        [System.Text.RegularExpressions.GeneratedRegex(@"^(http?|https):\/\/[^\s\/$.?#].[^\s]*$")]
        private static partial System.Text.RegularExpressions.Regex WebsiteUrlRegex();

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
                < 0 => $"Closed on {closingDate:dddd d MMMM}",
                0 => $"Closes today{timeSuffix}",
                1 => $"Closes tomorrow ({closingDate:dddd d MMMM}{timeSuffix})",
                <= 31 => $"Closes in {daysToExpiry} days ({closingDate:dddd d MMMM}{timeSuffix})",
                _ => $"Closes on {closingDate:dddd d MMMM}"
            };
        }
        
        public static string GetClosingDate(IDateTimeService dateTimeService, DateTime closingDate, DateTime? closedDate, bool isExternalVacancy = false)
        {
            return closedDate.HasValue
                ? $"Closed on {closedDate:dddd d MMMM}"
                : GetClosingDate(dateTimeService, closingDate, isExternalVacancy);    
        }

        public static int GetDaysUntilExpiry(IDateTimeService dateTimeService, DateTime closingDate)
        {
            var timeUntilClosing = closingDate.Date - dateTimeService.GetDateTime();
            return (int)Math.Ceiling(timeUntilClosing.TotalDays);
        }

        public static string? FormatEmployerWebsiteUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            return !WebsiteUrlRegex().IsMatch(url) ? $"http://{url}" : url;
        }

        public static string GetPostedDate(this DateTime postedDate)
        {
            return $"Posted on {postedDate.ToString("d MMMM yyyy", CultureInfo.InvariantCulture)}";
        }

        public static string GetStartDate(this DateTime startDate)
        {
            return startDate.ToString("dddd d MMMM", CultureInfo.InvariantCulture);
        }

        public static string GetMapsPostedDate(this DateTime postedDate)
        {
            return $"Posted on {postedDate.ToString("d MMMM", CultureInfo.InvariantCulture)}";
        }

        public static string GetWageText(string wageAmountText)
        {
            if (string.IsNullOrEmpty(wageAmountText)) return $"{wageAmountText}";

            byte[] encodedBytes = Encoding.UTF8.GetBytes(wageAmountText);
            string decoded = Encoding.UTF8.GetString(encodedBytes);

            var poundRemovedStr = decoded
                .Replace("£", string.Empty)
                .Replace("\u00A3", string.Empty)
                .Replace("u+00A3", string.Empty);

            if (!decimal.TryParse(poundRemovedStr, out var wageAmount)) return $"{wageAmountText}";

            return wageAmount switch
            {
                < 100.00M => wageAmount % 1 == 0 ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##} an hour", wageAmount) : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00} an hour", wageAmount),
                > 5000.00M => wageAmount % 1 == 0 ? string.Format(CultureInfo.InvariantCulture, "£{0:#,##} a year", wageAmount) : string.Format(CultureInfo.InvariantCulture, "£{0:#,##.00} a year", wageAmount),
                _ => wageAmountText
            };
        }
    }
}