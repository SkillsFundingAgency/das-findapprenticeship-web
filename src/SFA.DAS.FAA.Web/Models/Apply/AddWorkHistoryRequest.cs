using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddWorkHistoryRequest : ViewModelBase
    {
        [FromRoute] public required string VacancyReference { get; init; }
        [FromRoute] public required Guid ApplicationId { get; init; }
        [BindProperty] public string AddJob { get; init; }
        public string[] Jobs = ["Yes", "No"];
        public string? BackLinkUrl { get; set; }
    }
}
