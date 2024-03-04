using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class InterviewAdjustmentsViewModel
{
    public InterviewAdjustmentsViewModel()
    {
        
    }

    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public bool? DoYouWantInterviewAdjustments { get; set; }
    public string? InterviewAdjustmentsDescription { get; set; }
}
