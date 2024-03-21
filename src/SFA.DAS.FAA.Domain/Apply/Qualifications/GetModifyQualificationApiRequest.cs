using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetModifyQualificationApiRequest(Guid qualificationReferenceId, Guid applicationId,Guid candidateId, Guid? qualificationId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify?candidateId={candidateId}&id={qualificationId}";
}

public class GetModifyQualificationApiResponse
{
    public QualificationTypeApiResponse QualificationType { get; set; }
    public List<GetQualificationsApiResponse.Qualification> Qualifications { get; set; } = [];
}