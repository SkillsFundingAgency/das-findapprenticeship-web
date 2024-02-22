using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.ModelBinding;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddVolunteeringAndWorkExperienceViewModel : AddVolunteeringAndWorkExperienceViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; init; }
    }

    public abstract class AddVolunteeringAndWorkExperienceViewModelBase
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
