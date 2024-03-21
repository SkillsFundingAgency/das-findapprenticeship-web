using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.Qualifications
{
    public class PostDeleteQualificationsApiRequest(Guid applicationId, Guid qualificationType, PostDeleteQualificationsApiRequest.PostDeleteQualificationsApiRequestBody body) : IPostApiRequest
    {
        public string PostUrl => $"applications/{applicationId}/qualifications/delete/{qualificationType}";
        public object Data { get; set; } = body;

        public class PostDeleteQualificationsApiRequestBody
        {
            public Guid CandidateId { get; set; }
        }
    }
}
