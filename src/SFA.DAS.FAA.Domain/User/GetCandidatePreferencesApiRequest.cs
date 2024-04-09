using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetCandidatePreferencesApiRequest : IGetApiRequest
{
    private readonly Guid _candidateId;

    public GetCandidatePreferencesApiRequest(Guid candidateId)
    {
        _candidateId = candidateId;
    }

    public string GetUrl => $"users/{_candidateId}/create-account/candidate-preferences";
}
