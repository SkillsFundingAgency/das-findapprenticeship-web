using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.ModelBinding;

namespace SFA.DAS.FAA.Web.Models.Custom
{
    [ModelBinder(BinderType = typeof(MonthYearDateModelBinder))]
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
