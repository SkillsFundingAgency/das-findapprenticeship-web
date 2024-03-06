using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
public class UpdateInterviewAdjustmentsCommand : IRequest<UpdateInterviewAdjustmentsCommandResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string? InterviewAdjustmentsDescription { get; set; }
    public SectionStatus InterviewAdjustmentsSectionStatus { get; set; }
}
