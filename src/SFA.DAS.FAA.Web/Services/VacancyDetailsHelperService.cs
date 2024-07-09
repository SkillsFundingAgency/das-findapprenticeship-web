using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;

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

        public static string GetClosingDate(IDateTimeService dateTimeService, DateTime closingDate)
        {
            var daysToExpiry = GetDaysUntilExpiry(dateTimeService, closingDate);

            return daysToExpiry switch
            {
                < 0 => $"Closed on {closingDate:dddd d MMMM}",
                0 => "Closes today at 11:59pm",
                1 => $"Closes tomorrow ({closingDate:dddd d MMMM} at 11:59pm)",
                <= 31 => $"Closes in {daysToExpiry} days ({closingDate:dddd d MMMM} at 11:59pm)",
                _ => $"Closes on {closingDate:dddd d MMMM}"
            };
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
    }
}