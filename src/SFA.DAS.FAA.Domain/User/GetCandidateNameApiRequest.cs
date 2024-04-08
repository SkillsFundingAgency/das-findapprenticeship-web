using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetCandidateNameApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"users/{candidateId}/user-name";
}