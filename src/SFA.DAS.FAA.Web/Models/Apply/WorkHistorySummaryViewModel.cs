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
        public List<SectionCompleted> ApplicationCompletionStatus { get; init; } =
        [
            new SectionCompleted
            {
                Id = SectionStatus.Completed,
                StatusText = "Yes, I’ve completed this section"
            },

            new SectionCompleted
            {
                Id = SectionStatus.InProgress,
                StatusText = "No, I’ll come back to it later"
            }
        ];
    }
}
