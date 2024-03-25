using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetModifyQualification;

public class GetModifyQualificationQueryResult
{
    public QualificationTypeApiResponse? QualificationType { get; set; }
    public List<GetQualificationsApiResponse.Qualification>? Qualifications { get; set; }
    public List<CourseApiResponse> Courses { get; set; }
}