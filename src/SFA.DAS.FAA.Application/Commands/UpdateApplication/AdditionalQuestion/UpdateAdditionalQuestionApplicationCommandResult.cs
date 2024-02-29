namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.AdditionalQuestion;

public record UpdateAdditionalQuestionApplicationCommandResult
{
    public Domain.Apply.UpdateApplication.Application? Application { get; set; }
}