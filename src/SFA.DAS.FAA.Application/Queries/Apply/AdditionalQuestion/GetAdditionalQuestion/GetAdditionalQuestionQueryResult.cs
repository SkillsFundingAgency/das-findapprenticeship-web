using SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;

namespace SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;

public record GetAdditionalQuestionQueryResult
{
    public Guid Id { get; private init; }
    public string? QuestionText { get; private init; }
    public string? Answer { get; private init; }
    public Guid ApplicationId { get; private init; }

    public static implicit operator GetAdditionalQuestionQueryResult(GetAdditionalQuestionApiResponse source)
    {
        return new GetAdditionalQuestionQueryResult
        {
            Answer = source.Answer,
            QuestionText = source.QuestionText,
            ApplicationId = source.ApplicationId,
            Id = source.Id,
        };
    }
}