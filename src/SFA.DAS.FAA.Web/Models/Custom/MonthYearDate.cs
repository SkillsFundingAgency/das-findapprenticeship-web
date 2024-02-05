namespace SFA.DAS.FAA.Web.Models.Custom
{
    public class MonthYearDate
    {
        public MonthYearDate(string value)
        {
            SetValue(value);
        }

        public MonthYearDate(DateTime? value)
        {
            DateTimeValue = value;
        }

        public void SetValue(DateTime value)
        {
            DateTimeValue = value; 
        }

        public void SetValue(string value)
        {
            if (DateTime.TryParse(value, out var parsedValue))
            {
                DateTimeValue = parsedValue;
            }
            else
            {
                DateTimeValue = null;
            }
        }

        public DateTime? DateTimeValue { get; set; }
    }
}
