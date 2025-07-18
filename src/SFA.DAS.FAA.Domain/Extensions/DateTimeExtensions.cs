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
    /// Formats a DateTime to a GDS date string with time.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToGdsDateStringWithTime(this DateTime dateTime)
    {
        string formatted = dateTime.ToString("h:mmtt 'on' dddd d MMMM yyy", CultureInfo.InvariantCulture);
        return formatted
            .Replace("AM", "am")
            .Replace("PM", "pm");
    }

    /// <summary>
    /// Formats a DateTime to a GDS date string with time and timezone.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToGdsDateStringWithTimeZone(this DateTime dateTime)
    {
        string formatted = dateTime.ToString("h:mmtt '(GMT)' 'on' dddd d MMMM yyy", CultureInfo.InvariantCulture);
        return formatted
            .Replace("AM", "am")
            .Replace("PM", "pm");
    }
}