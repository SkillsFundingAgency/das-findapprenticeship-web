using SFA.DAS.FAT.Domain.Interfaces;
using System.Globalization;

namespace SFA.DAS.FAA.Web.Services
{
    public static class VacancyDetailsService
    {
        public static string GetWorkingHours(this string? duration)
        {
            if (decimal.TryParse(duration, out var workHours))
            {
                var hrs = workHours.ToString("0.00", CultureInfo.InvariantCulture);
                var parts = hrs.Split('.');
                var minutes = int.Parse(parts[1]);
                return minutes > 0 ? $"{parts[0]} hours {minutes} minutes" : $"{parts[0]} hours";
            }
           
            return "0 hours";
        }

        public static string GetClosingDate(IDateTimeService dateTimeService, DateTime closingDate)
        {
            var currentDate = dateTimeService.GetDateTime();
            if (closingDate < currentDate.AddDays(31))
            {
                var timeUntilClosing = closingDate.Date - currentDate;
                var days = (int)Math.Ceiling(timeUntilClosing.TotalDays);
                return $"Closes in {days} ({closingDate:dddd, dd, MMMM} at 11.59pm)";
            }
            return $"Closes on {closingDate:dddd, dd, MMMM}";
        }

        public static string GetPostedDate(this DateTime postedDate)
        {
            return $"Posted on {postedDate:dd MMMM}";
        }

        public static string GetStartDate(this DateTime startDate)
        {
            return $"{startDate:dddd dd MMMM}";
        }
    }
}
