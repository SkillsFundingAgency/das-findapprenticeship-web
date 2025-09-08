using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class InterviewAdjustmentSummaryViewModel : ViewModelBase
{
    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public bool IsSupportRequestRequired { get; init; }
    public string? SupportRequestAnswer { get; init; }

    [BindProperty]
    public bool? IsSectionCompleted { get; set; }
    public string? BackLinkUrl { get; set; }
}