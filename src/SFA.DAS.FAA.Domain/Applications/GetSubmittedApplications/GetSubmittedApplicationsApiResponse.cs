using Newtonsoft.Json;

namespace SFA.DAS.FAA.Domain.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsApiResponse
    {
        [JsonProperty("submittedApplications")]
        public List<SubmittedApplication> SubmittedApplications { get; set; } = [];

        public class SubmittedApplication
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }

            [JsonProperty("title")]
            public string? Title { get; set; }

            [JsonProperty("vacancyReference")]
            public string? VacancyReference { get; set; }

            [JsonProperty("employerName")]
            public string? EmployerName { get; set; }

            [JsonProperty("city")]
            public string? City { get; set; }

            [JsonProperty("postcode")]
            public string? Postcode { get; set; }

            [JsonProperty("createdDate")]
            public DateTime CreatedDate { get; set; }

            [JsonProperty("submittedDate")]
            public DateTime? SubmittedDate { get; set; }

            [JsonProperty("closingDate")]
            public DateTime ClosingDate { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }
        }
    }
}
