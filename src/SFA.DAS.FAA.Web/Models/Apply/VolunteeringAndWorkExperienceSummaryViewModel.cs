using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public record VolunteeringAndWorkExperienceSummaryViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; init; }
        public string? BackLinkUrl { get; init; }
        public string? AddAnotherVolunteeringAndWorkExperienceLinkUrl { get; init; }
        [BindProperty]
        public bool? IsSectionCompleted { get; init; }
        public List<WorkHistoryViewModel> WorkHistories { get; init; } = [];
    }
}
