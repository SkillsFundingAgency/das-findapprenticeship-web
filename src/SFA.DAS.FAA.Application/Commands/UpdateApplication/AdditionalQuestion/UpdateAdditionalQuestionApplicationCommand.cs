using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.AdditionalQuestion;

public record UpdateAdditionalQuestionApplicationCommand : IRequest<UpdateAdditionalQuestionApplicationCommandResult>
{
    public required Guid ApplicationId { get; init; }
    public required Guid CandidateId { get; init; }
    public SectionStatus AdditionQuestionOne { get; init; }
    public SectionStatus AdditionQuestionTwo { get; init; }
}