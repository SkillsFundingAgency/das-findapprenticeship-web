using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddWorkHistoryViewModel : ViewModelBase
    {
        [FromRoute] 
        public required Guid ApplicationId { get; init; }
        [BindProperty] 
        public string? AddJob { get; set; }
        public string[] Jobs = ["Yes", "No"];
        public string? BackLinkUrl { get; set; }
    }
}
