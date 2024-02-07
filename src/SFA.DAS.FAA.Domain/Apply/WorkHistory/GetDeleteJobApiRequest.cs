using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory
{
    public class GetDeleteJobApiRequest(Guid ApplicationId, Guid CandidateId, Guid JobId) : IGetApiRequest
    {
        public string GetUrl => $"applications/{ApplicationId}/jobs/{JobId}/delete?candidateId={CandidateId}";
    }
    public class GetDeleteJobApiResponse
    {
        public Guid Id { get; set; }
        public string Employer { get; set; }
        public string JobTitle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ApplicationId { get; set; }
        public string Description { get; set; }
    }
}
