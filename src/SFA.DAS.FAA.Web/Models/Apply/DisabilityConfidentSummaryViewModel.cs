using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class DisabilityConfidentSummaryViewModel : ViewModelBase
    {
        [FromRoute]
        public required Guid ApplicationId { get; init; }
        public bool IsApplyUnderDisabilityConfidentSchemeRequired { get; init; }

        [BindProperty]
        public bool? IsSectionCompleted { get; init; }
    }
}
