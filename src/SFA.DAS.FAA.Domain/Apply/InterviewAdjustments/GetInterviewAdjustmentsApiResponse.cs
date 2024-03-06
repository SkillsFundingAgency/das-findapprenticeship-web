namespace SFA.DAS.FAA.Domain.Apply.InterviewAdjustments;

public record GetInterviewAdjustmentsApiResponse
{
    public Guid ApplicationId { get; set; }
    public string? InterviewAdjustmentsDescription { get; set; }
    public bool? Status { get; set; }
}