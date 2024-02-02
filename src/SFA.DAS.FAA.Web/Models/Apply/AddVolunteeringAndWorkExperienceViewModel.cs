using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddVolunteeringAndWorkExperienceViewModel : ViewModelBase
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public string? BackLinkUrl { get; set; }
    [BindProperty]
    public string? AddVolunteeringAndWorkExperience { get; set; }
    public string[] VolunteeringAndWorkExperience = ["Yes", "No"];
}
