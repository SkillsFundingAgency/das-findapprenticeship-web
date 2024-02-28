using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetVolunteeringOrWorkExperienceItem;
using SFA.DAS.FAA.Web.Models.Apply.Base;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply
{
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
