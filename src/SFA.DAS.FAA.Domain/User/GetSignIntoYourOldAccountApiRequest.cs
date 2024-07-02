using System.Web;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public class GetSignIntoYourOldAccountApiRequest(Guid candidateId, string email, string password) : IGetApiRequest
    {
        public string GetUrl => $"candidates/sign-in-to-your-old-account?candidateId={candidateId}&email={HttpUtility.UrlEncode(email)}&password={HttpUtility.UrlEncode(password)}";
    }

    public class GetSignIntoYourOldAccountApiResponse
    {
        public bool IsValid { get; set; }
    }
}
