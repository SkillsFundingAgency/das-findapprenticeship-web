using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetModifyQualificationApiRequest(Guid qualificationReferenceId, Guid applicationId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications/{qualificationReferenceId}/modify";
}

public class GetModifyQualificationApiResponse
{
    public QualificationTypeApiResponse QualificationType { get; set; }  
}