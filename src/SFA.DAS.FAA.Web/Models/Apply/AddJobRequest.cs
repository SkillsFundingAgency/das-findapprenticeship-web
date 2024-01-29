using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddJobRequest
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
    }

    public class AddJobViewModel : AddJobPostRequest
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
    }

    public class AddJobPostRequest
    {
        public string EmployerName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string StartDateMonth { get; set; }
        public string StartDateYear { get; set; }
        public bool IsCurrentRole { get; set; }
        public string EndDateMonth { get; set;}
        public string EndDateYear { get; set;}
    }
}
