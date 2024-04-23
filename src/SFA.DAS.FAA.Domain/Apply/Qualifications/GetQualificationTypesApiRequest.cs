using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetQualificationTypesApiRequest(Guid applicationId, Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications/add/select-type?candidateId={candidateId}";
}

public class GetQualificationTypesApiResponse
{
    public bool HasAddedQualifications { get; set; }
    public List<QualificationTypeApiResponse> QualificationTypes { get; set; }
}