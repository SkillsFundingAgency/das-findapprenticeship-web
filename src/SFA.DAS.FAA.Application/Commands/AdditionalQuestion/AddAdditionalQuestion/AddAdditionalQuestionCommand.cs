using MediatR;

namespace SFA.DAS.FAA.Application.Commands.AdditionalQuestion.AddAdditionalQuestion;

public record AddAdditionalQuestionCommand : IRequest<AddAdditionalQuestionCommandHandlerResult>
{
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string? Answer { get; set; }
    public Guid Id { get; set; }
}