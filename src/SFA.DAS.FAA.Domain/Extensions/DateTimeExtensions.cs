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
}