using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class TrainingCoursesSummaryViewModel : ViewModelBase
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public List<AddTrainingCourseViewModel> TrainingCourses { get; set; } = [];
    public string? BackLinkUrl { get; set; }
    public string? DeleteLinkUrl { get; set; }
    public string? ChangeLinkUrl { get; set; }
    public string? AddAnotherTrainingCourseLinkUrl { get; set; }

    [Required(ErrorMessage = "Select if you have finished this section")]
    public bool? IsSectionCompleted { get; set; }
}
