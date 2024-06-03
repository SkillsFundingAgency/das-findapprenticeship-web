using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Applications.GetLegacyApplications
{
    public record GetLegacyApplicationsApiRequest(string EmailAddress) : IGetApiRequest
    {
        public string GetUrl => $"applications/legacy?emailAddress={EmailAddress}";
    }
}