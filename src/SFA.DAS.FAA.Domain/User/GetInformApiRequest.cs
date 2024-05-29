using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public class GetInformApiRequest(Guid candidateId) : IGetApiRequest
    {
        public string GetUrl => $"users/create-account?candidateId={candidateId}";
    }

    public class GetInformApiResponse
    {
        public bool ShowAccountRecoveryBanner { get; set; }
    }
}
