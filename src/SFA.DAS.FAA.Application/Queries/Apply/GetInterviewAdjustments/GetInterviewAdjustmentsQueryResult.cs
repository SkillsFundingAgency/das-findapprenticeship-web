using SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
public record GetInterviewAdjustmentsQueryResult
{
    public string? InterviewAdjustmentsDescription { get; set; }
    public bool? Status { get; set; }

    public static implicit operator GetInterviewAdjustmentsQueryResult(GetInterviewAdjustmentsApiResponse source)
    {
        return new GetInterviewAdjustmentsQueryResult
        {
            InterviewAdjustmentsDescription = source.InterviewAdjustmentsDescription,
            Status = source.Status
        };
    }
}
