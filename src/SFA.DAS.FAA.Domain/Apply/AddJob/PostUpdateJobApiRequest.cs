using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.AddJob
{
    public class PostUpdateJobApiRequest(Guid applicationId, Guid jobId, PostUpdateJobApiRequest.PostUpdateJobApiRequestData body)
        : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/jobs/{jobId}";
        public object Data { get; set; } = body;

        public class PostUpdateJobApiRequestData
        {
            public Guid CandidateId { get; set; }
            public string EmployerName { get; set; }
            public string JobTitle { get; set; }
            public string JobDescription { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }
    }
}
