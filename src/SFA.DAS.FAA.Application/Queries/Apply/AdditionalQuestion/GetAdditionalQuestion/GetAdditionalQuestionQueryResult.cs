using SFA.DAS.FAA.Domain.Apply.GetAdditionalQuestion;

namespace SFA.DAS.FAA.Application.Queries.Apply.AdditionalQuestion.GetAdditionalQuestion;

public record GetAdditionalQuestionQueryResult
{
    public Guid Id { get; set; }
    public string? QuestionText { get; set; }
    public string? Answer { get; set; }
    public Guid ApplicationId { get; set; }

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