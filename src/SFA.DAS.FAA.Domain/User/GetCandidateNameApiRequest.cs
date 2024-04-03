using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetCandidateNameApiRequest : IGetApiRequest
{
    private readonly string _govUkIdentifier;

    public GetCandidateNameApiRequest(string govUkIdentifier)
    {
        _govUkIdentifier = govUkIdentifier;
    }

    public string GetUrl => $"users/{_govUkIdentifier}/user-name";
}