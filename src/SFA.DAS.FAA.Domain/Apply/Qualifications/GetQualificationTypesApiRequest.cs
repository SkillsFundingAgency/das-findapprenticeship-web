using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications;

public class GetQualificationTypesApiRequest : IGetApiRequest
{
    public string GetUrl => "referencedata/qualificationtypes";
}

public class GetQualificationTypesApiResponse
{
    public List<QualificationTypeApiResponse> QualificationTypes { get; set; }
}