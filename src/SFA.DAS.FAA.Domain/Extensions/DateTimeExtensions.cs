using System.Globalization;

namespace SFA.DAS.FAA.Domain.Extensions;

public static class DateTimeExtensions
{
    public static string ToGdsDateString(this DateTime dateTime)
    {
        return dateTime.ToString("d MMMM yyy", CultureInfo.InvariantCulture);
    }
    
    public static string ToGdsDateStringWithDayOfWeek(this DateTime dateTime)
    {
        return dateTime.ToString("dddd d MMMM yyy", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Formats a DateTime to a UK time string with DST label.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToUkTimeWithDstLabel(this DateTime dateTime)
    {
        TimeZoneInfo ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        DateTime ukTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), ukTimeZone);

        string suffix = ukTimeZone.IsDaylightSavingTime(ukTime) ? "BST" : "GMT";

        string formatted = ukTime.ToString("h:mmtt 'on' dddd d MMMM yyy", CultureInfo.InvariantCulture);

        return formatted
                .Replace("AM", "am")
                .Replace("PM", "pm") + $" {suffix}";
    }
}