using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory
{
    public class PostDeleteJobApiRequest(Guid applicationId, Guid jobId, PostDeleteJobApiRequest.PostDeleteJobApiRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/jobs/{jobId}/delete";
        public object Data { get; set; } = body;

        public class PostDeleteJobApiRequestData
        {
            public Guid CandidateId { get; set; }

        }
    }
}
