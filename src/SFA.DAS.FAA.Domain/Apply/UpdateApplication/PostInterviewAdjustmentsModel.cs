using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.Apply.UpdateApplication;
public record PostInterviewAdjustmentsModel
{
    public Guid CandidateId { get; set; }
    public string? InterviewAdjustmentsDescription { get; set; }
    public SectionStatus InterviewAdjustmentsSectionStatus { get; set; }
}
