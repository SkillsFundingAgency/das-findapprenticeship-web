using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;

public class UpdateCheckAnswersApiRequest(Guid candidateId) : IPostApiRequest
{
    public string PostUrl => $"users/{candidateId}/create-account/check-answers";
    public object Data { get; set; }
}