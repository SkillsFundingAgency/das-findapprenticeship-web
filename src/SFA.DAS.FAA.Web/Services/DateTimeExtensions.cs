namespace SFA.DAS.FAA.Web.Services
{
    public static class DateTimeExtensions
    {
        public static string ToApiString(this DateTime date) => date.ToString("yyyy-MM-dd");
    }
}
