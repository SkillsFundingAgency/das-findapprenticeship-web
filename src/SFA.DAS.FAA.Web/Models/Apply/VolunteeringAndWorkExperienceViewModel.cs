using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class VolunteeringAndWorkExperienceViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public string? BackLinkUrl { get; set; }
    public bool? DoYouWantToAddAnyVolunteeringAndWorkExperience { get; set; }
}
