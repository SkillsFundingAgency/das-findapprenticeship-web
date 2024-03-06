using SFA.DAS.FAA.Domain.Apply.InterviewAdjustments;

namespace SFA.DAS.FAA.Application.Queries.Apply.InterviewAdjustments;

public class GetInterviewAdjustmentsQueryResult
{
    public Guid ApplicationId { get; set; }
    public string? InterviewAdjustmentsDescription { get; init; }
    public bool Status { get; init; }

    public static implicit operator GetInterviewAdjustmentsQueryResult(GetInterviewAdjustmentsApiResponse source)
    {
        return new GetInterviewAdjustmentsQueryResult
        {
            ApplicationId = source.ApplicationId,
            InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription,
            Status = source.Status ?? false
        };
    }
}