using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;

public class GetCandidateCheckAnswersApiRequest(string govUkIdentifier) : IGetApiRequest
{
    public string GetUrl => $"users/{govUkIdentifier}/create-account/check-answers";
}