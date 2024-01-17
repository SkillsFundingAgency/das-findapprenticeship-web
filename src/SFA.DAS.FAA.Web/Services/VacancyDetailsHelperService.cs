using SFA.DAS.FAT.Domain.Interfaces;

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
            var currentDate = dateTimeService.GetDateTime();
            if (closingDate < currentDate.AddDays(31))
            {
                var timeUntilClosing = closingDate.Date - currentDate;
                var days = (int) Math.Ceiling(timeUntilClosing.TotalDays);
                return days > 1
                    ? $"Closes in {days} days ({closingDate:dddd dd MMMM} at 11.59pm)"
                    : $"Closes in {days} day ({closingDate:dddd dd MMMM} at 11.59pm)";
            }

            return $"Closes on {closingDate:dddd dd MMMM}";
        }

        public static string GetApplyNowClosingDate(IDateTimeService dateTimeService, DateTime closingDate)
        {
            var currentDate = dateTimeService.GetDateTime();
            var timeUntilClosing = closingDate.Date - currentDate;
            var days = (int) Math.Ceiling(timeUntilClosing.TotalDays);
            return days > 1 ? $"Closing in {days} days" : $"Closing in {days} day";
        }

        public static string? FormatEmployerWebsiteUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return url;
            return !WebsiteUrlRegex().IsMatch(url) ? $"http://{url}" : url;
        }

        public static string GetPostedDate(this DateTime postedDate)
        {
            return $"Posted on {postedDate:dd MMMM yyyy}";
        }

        public static string GetStartDate(this DateTime startDate)
        {
            return $"{startDate:dddd dd MMMM}";
        }
    }
}