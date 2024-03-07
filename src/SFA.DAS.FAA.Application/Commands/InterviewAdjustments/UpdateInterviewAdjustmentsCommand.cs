using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
public record UpdateInterviewAdjustmentsCommand : IRequest<UpdateInterviewAdjustmentsCommandResult>
{
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; init; }
    public string? InterviewAdjustmentsDescription { get; init; }
    public SectionStatus InterviewAdjustmentsSectionStatus { get; init; }
}
