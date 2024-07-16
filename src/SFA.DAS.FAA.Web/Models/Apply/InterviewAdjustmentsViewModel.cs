using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class InterviewAdjustmentsViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public bool? DoYouWantInterviewAdjustments { get; set; }
    public string? InterviewAdjustmentsDescription { get; set; }
    public string? BackLinkUrl { get; set; }
}
