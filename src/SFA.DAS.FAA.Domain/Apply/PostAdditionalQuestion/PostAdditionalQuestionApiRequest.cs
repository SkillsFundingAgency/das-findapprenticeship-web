using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Apply.PostAdditionalQuestion.PostAdditionalQuestionApiRequest;

namespace SFA.DAS.FAA.Domain.Apply.PostAdditionalQuestion;

public record PostAdditionalQuestionApiRequest(Guid ApplicationId, PostAdditionalQuestionApiRequestData Body)
    : IPostApiRequest
{
    public string PostUrl => $"applications/{ApplicationId}/additionalquestions";
    public object Data { get; set; } = Body;

    public record PostAdditionalQuestionApiRequestData
    {
        public Guid CandidateId { get; set; }
        public string? Answer { get; set; }
        public Guid Id { get; set; }
    }
}