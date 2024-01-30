namespace SFA.DAS.FAA.Web.Models.Custom
{
    public class MonthYearDate
    {
        public MonthYearDate(string value)
        {
            if (DateTime.TryParse(value, out var parsedValue))
            {
                DateTimeValue = parsedValue;
            }

            DateTimeValue = null;
        }

        public DateTime? DateTimeValue { get; set; }
    }
}
