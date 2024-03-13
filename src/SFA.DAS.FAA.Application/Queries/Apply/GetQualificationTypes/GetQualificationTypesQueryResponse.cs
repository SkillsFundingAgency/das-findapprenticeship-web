using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetQualificationTypes;

public class GetQualificationTypesQueryResponse
{
    public List<QualificationTypeApiResponse> QualificationTypes { get; set; }
}