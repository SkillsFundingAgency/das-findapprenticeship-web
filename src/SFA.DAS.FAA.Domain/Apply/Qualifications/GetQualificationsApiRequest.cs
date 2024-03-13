using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetQualificationsApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications?candidateId={candidateId}";
}

public class GetQualificationsApiResponse
{
    public bool? IsSectionCompleted { get; set; }
    public List<Qualification> Qualifications { get; set; } = null!;

    public class Qualification
    {
    }
}