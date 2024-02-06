using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.AddJob
{
    public class PostJobApiRequest(Guid applicationId, PostJobApiRequest.PostJobApiRequestData body)
        : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/jobs";
        public object Data { get; set; } = body;

        public class PostJobApiRequestData
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
