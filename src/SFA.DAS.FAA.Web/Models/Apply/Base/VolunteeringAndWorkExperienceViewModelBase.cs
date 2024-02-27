using SFA.DAS.FAA.Web.ModelBinding;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply.Base
{
    public abstract class VolunteeringAndWorkExperienceViewModelBase
    {
        public string? CompanyName { get; init; }
        public string? Description { get; init; }
        [ModelBindingError("Enter a real date for the start date")]
        public MonthYearDate? StartDate { get; init; }
        public bool? IsCurrentRole { get; init; }
        [ModelBindingError("Enter a real date for the end date")]
        public MonthYearDate? EndDate { get; init; }
    }
}
