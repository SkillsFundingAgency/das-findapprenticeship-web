using Newtonsoft.Json;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Interfaces;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Domain.SavedVacancies
{
    public class GetSavedVacanciesApiRequest(Guid candidateId): IGetApiRequest
    {
        public string GetUrl => $"saved-vacancies?candidateId={candidateId}";
    }

    public class GetSavedVacanciesApiResponse
    {
        public List<SavedVacancy> SavedVacancies { get; set; } = [];

        public class SavedVacancy
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string VacancyReference { get; set; } = string.Empty;
            public string EmployerName { get; set; } = string.Empty;
            public DateTime CreatedDate { get; set; }
            public DateTime ClosingDate { get; set; }
            public Address Address { get; set; }
            public List<Address> OtherAddresses { get; set; } = [];
            public string? EmploymentLocationInformation { get; set; }
            public AvailableWhere? EmployerLocationOption { get; set; }
            public bool IsExternalVacancy { get; set; }
            public string ExternalVacancyUrl { get; set; }
            [JsonProperty("applicationStatus")]
            public ApplicationStatus? Status { get; set; }
        }
    }
}
