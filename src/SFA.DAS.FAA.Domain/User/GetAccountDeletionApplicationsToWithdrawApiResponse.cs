using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.User
{
    public record GetAccountDeletionApplicationsToWithdrawApiResponse
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
            [JsonProperty("address")]
            public Address Address { get; set; }
            [JsonProperty("otherAddresses")]
            public List<Address> OtherAddresses { get; set; } = [];
            [JsonProperty("employmentLocationInformation")]
            public string? EmploymentLocationInformation { get; set; }
            [JsonProperty("employerLocationOption")]
            public AvailableWhere? EmployerLocationOption { get; set; }

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
