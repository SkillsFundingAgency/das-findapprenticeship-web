using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory
{
    public class PostDeleteJobApiRequest(Guid jobId, Guid applicationId, PostDeleteJobApiRequest.PostDeleteJobApiRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/work-history/{jobId}";
        public object Data { get; set; } = body;

        public class PostDeleteJobApiRequestData
        {
            public Guid CandidateId { get; set; }

        }
    }
}
