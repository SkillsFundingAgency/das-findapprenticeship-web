using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetQualificationTypesApiRequest(Guid applicationId) : IGetApiRequest
{
    public string GetUrl => $"applications/{applicationId}/qualifications/add/select-type";
}

public class GetQualificationTypesApiResponse
{
    public List<QualificationTypeApiResponse> QualificationTypes { get; set; }
}