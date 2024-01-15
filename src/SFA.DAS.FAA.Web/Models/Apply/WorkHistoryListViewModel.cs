using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class WorkHistoryListViewModel
    {
        [BindProperty]
        public string AddJob { get; set; }
        public string[] Jobs = ["Yes", "No"];
    }
}
