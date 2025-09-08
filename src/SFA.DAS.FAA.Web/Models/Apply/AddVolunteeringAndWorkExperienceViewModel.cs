using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Models.Apply.Base;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddVolunteeringAndWorkExperienceViewModel : VolunteeringAndWorkExperienceViewModelBase
{
    [FromRoute]
    public Guid ApplicationId { get; init; }
}