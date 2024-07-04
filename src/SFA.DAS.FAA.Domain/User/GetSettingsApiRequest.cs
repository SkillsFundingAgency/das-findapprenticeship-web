using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User;

public class GetSettingsApiRequest(Guid candidateId, string email) : IGetApiRequest
{
    public string GetUrl => $"users/settings?candidateId={candidateId}&email={HttpUtility.UrlEncode(email)}";
}