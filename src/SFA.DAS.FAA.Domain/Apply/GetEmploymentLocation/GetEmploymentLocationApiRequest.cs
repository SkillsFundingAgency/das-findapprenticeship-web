using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.GetEmploymentLocation;

public record GetEmploymentLocationApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{ApplicationId}/employmentLocations?candidateId={CandidateId}";
}