using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class DisabilityConfidentViewModel
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
        public string? EmployerName { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
    }
}
