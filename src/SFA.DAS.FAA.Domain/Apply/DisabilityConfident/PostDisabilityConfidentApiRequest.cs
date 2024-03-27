using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.DisabilityConfident
{
    public class PostDisabilityConfidentApiRequest(Guid applicationId, PostDisabilityConfidentApiRequest.PostDisabilityConfidentApiRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/disability-confident";
        public object Data { get; set; } = body;

        public class PostDisabilityConfidentApiRequestData
        {
            public Guid CandidateId { get; set; }
            public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        }
    }
}
