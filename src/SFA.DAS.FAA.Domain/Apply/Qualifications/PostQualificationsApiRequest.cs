using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications
{
    public class PostQualificationsApiRequest(Guid applicationId, PostQualificationsApiRequest.PostQualificationsApiRequestData body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/qualifications";
        public object Data { get; set; } = body;

        public class PostQualificationsApiRequestData
        {
            public Guid CandidateId { get; set; }
            public bool IsComplete { get; set; }
        }
    }
}
