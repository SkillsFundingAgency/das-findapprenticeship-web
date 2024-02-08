using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class TrainingCoursesViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public bool? DoYouWantToAddAnyTrainingCourses { get; set; }
    public string? BackLinkUrl { get; set; }
}
