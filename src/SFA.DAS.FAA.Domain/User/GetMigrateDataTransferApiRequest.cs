using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public record GetMigrateDataTransferApiRequest(string EmailAddress, Guid CandidateId) : IGetApiRequest
    {
        public string GetUrl => $"users/migrate?emailAddress={HttpUtility.UrlEncode(EmailAddress)}&candidateId={CandidateId}";
    }
}