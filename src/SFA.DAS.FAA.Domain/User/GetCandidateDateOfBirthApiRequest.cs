using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;
public class GetCandidateDateOfBirthApiRequest : IGetApiRequest
{
    private readonly string _govUkIdentifier;

    public GetCandidateDateOfBirthApiRequest(string govUkIdentifier)
    {
        _govUkIdentifier = govUkIdentifier;
    }

    public string GetUrl => $"users/{_govUkIdentifier}/date-of-birth";
}
