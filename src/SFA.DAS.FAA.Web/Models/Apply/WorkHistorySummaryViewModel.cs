using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class WorkHistorySummaryViewModel : ViewModelBase
    {
        [FromRoute]
        public required Guid ApplicationId { get; init; }
        public string? BackLinkUrl { get; set; }
        public string? DeleteLinkUrl { get; set; }
        public string? ChangeLinkUrl { get; set; }
        public string? AddAnotherJobLinkUrl { get; set; }

        [BindProperty]
        public SectionStatus? IsSectionCompleted { get; set; }
        public List<WorkHistoryViewModel> WorkHistories { get; set; } = [];
        public List<SectionProgress> ApplicationCompletionStatus { get; init; } =
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
