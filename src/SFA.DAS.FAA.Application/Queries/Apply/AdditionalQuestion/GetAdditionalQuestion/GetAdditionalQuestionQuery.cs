using MediatR;

namespace SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;

public record GetAdditionalQuestionQuery : IRequest<GetAdditionalQuestionQueryResult>
{
    public Guid CandidateId { get; init; }
    public Guid ApplicationId { get; init; }
    public Guid AdditionalQuestionId { get; init; }
    public int AdditionalQuestion { get; init; }
}