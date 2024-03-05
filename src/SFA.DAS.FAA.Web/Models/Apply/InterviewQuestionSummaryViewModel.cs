using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class InterviewQuestionSummaryViewModel : ViewModelBase
    {
        [FromRoute]
        public required Guid ApplicationId { get; init; }
        public string? BackLinkUrl { get; set; }
        public string? ChangeLinkUrl { get; set; }
        public string? ChangeSupportRequestAnswerLinkUrl { get; set; }
        public bool IsSupportRequestRequired { get; set; }
        public string? SupportRequestAnswer { get; set; }

        [BindProperty]
        public SectionStatus? IsSectionCompleted { get; set; }
        public List<SectionProgress> InterviewQuestionCompletionStatus { get; init; } =
        [
            new SectionProgress
            {
                Id = SectionStatus.Completed,
                StatusLabel = "Yes, I’ve completed this section"
            },

            new SectionProgress
            {
                Id = SectionStatus.InProgress,
                StatusLabel = "No, I’ll come back to it later"
            }
        ];
    }
}
