using MediatR;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;

public record AddAdditionalQuestionCommand : IRequest<AddAdditionalQuestionCommandHandlerResult>
{
    public Guid ApplicationId { get; init; }
    public Guid CandidateId { get; init; }
    public string? Answer { get; init; }
    public Guid Id { get; init; }
    public required int UpdatedAdditionalQuestion { get; init; }
    public SectionStatus AdditionalQuestionSectionStatus { get; init; }
}