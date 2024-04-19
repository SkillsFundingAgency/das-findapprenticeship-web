using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;

public class GetCandidateCheckAnswersApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"users/{candidateId}/create-account/check-answers";
}