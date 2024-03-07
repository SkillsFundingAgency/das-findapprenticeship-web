namespace SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
public record UpdateInterviewAdjustmentsCommandResult
{
    public Domain.Apply.UpdateApplication.Application? Application { get; init; }
}
