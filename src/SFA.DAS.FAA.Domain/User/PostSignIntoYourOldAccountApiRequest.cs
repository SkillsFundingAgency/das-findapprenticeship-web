using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.User
{
    public record PostSignIntoYourOldAccountApiRequest(PostSignIntoYourOldAccountApiRequestData Body) : IPostApiRequest
    {
        public string PostUrl => "candidates/sign-in-to-your-old-account";
        public object Data { get; set; } = Body;
    }

    public class PostSignIntoYourOldAccountApiRequestData
    {
        public required Guid CandidateId { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class PostSignIntoYourOldAccountApiResponse
    {
        public bool IsValid { get; set; }
    }
}