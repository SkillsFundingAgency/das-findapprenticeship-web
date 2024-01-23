using SFA.DAS.FAA.Domain.Apply.UpsertApplication.Enums;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.UpsertApplication
{
    public class PatchApplicationApiRequest(
        string vacancyReference,
        Guid applicationId,
        Guid candidateId,
        UpdateApplicationModel data)
        : IPatchApiRequest
    {
        public object Data { get; set; } = data;

        public string PatchUrl => $"/vacancies/{vacancyReference}/apply/{applicationId}/{candidateId}";
    }

    public class UpdateApplicationModel
    {
        public SectionStatus WorkHistorySectionStatus { get; set; }
    }
}