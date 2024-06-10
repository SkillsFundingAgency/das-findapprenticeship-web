using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;

public class GetSettingsApiRequest(Guid candidateId) : IGetApiRequest
{
    public string GetUrl => $"user/settings?candidateId={candidateId}";
}