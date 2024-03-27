namespace SFA.DAS.FAA.Application.Commands.DisabilityConfident;

public record UpdateDisabilityConfidenceApplicationCommandResult
{
    public Domain.Apply.UpdateApplication.Application? Application { get; init; }
}