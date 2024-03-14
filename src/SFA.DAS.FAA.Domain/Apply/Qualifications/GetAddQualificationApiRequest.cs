using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetAddQualificationApiRequest(Guid qualificationReferenceId, Guid applicationId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications/{qualificationReferenceId}/add";
}

public class GetAddQualificationApiResponse
{
    public QualificationTypeApiResponse QualificationType { get; set; }  
}