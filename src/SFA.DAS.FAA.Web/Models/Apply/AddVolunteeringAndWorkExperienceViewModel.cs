using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Web.ModelBinding;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddVolunteeringAndWorkExperienceViewModel : VolunteeringAndWorkExperienceViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; init; }
    }

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

    public class EditVolunteeringAndWorkExperienceViewModel : VolunteeringAndWorkExperienceViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; init; }

        public Guid VolunteeringOrWorkExperienceId { get; init; }

        public static implicit operator EditVolunteeringAndWorkExperienceViewModel(GetVolunteeringOrWorkExperienceItemQueryResult source)
        {
            return new EditVolunteeringAndWorkExperienceViewModel
            {
                ApplicationId = source.ApplicationId,
                CompanyName = source.Organisation,
                StartDate = new MonthYearDate(source.StartDate),
                EndDate = new MonthYearDate(source.EndDate),
                IsCurrentRole = !source.EndDate.HasValue,
                Description = source.Description,
                VolunteeringOrWorkExperienceId = source.Id
            };
        }
    }
}
