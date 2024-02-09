using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddTrainingCourseViewModel : ViewModelBase
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    [BindProperty]
    public string? AddTrainingCourse { get; set; }
    public string[] TrainingCourse = ["Yes", "No"];
    public string? BackLinkUrl { get; set; }
}
