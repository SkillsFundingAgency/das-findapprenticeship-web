using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public record GetMigrateDataTransferApiRequest(string EmailAddress) : IGetApiRequest
    {
        public string GetUrl => $"users/migrate?emailAddress={EmailAddress}";
    }
}