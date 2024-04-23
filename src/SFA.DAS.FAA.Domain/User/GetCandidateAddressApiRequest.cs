using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetCandidateAddressApiRequest : IGetApiRequest
{
    private readonly Guid _candidateId;

    public GetCandidateAddressApiRequest(Guid candidateId)
    {
        _candidateId = candidateId;
    }

    public string GetUrl => $"users/{_candidateId}/create-account/user-address";
}
