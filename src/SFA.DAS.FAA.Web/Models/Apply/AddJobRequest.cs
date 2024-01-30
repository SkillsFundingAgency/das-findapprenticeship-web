using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Models.Custom;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddJobViewModel
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
        public string? JobTitle { get; set; }
        public string? EmployerName { get; set; }
        public string? JobDescription { get; set; }
        public MonthYearDate? StartDate { get; set; }
        public bool? IsCurrentRole { get; set; }
        public MonthYearDate? EndDate { get; set; }
    }
}
