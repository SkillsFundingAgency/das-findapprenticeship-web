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

public class QualificationTypeApiResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
}