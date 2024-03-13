using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetQualificationsApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{ApplicationId}/qualifications?candidateId={CandidateId}";
}

public class GetQualificationsApiResponse
{
    public bool? IsSectionCompleted { get; set; }
    public List<Qualification> Qualifications { get; set; } = null!;

    public class Qualification
    {
    }
}