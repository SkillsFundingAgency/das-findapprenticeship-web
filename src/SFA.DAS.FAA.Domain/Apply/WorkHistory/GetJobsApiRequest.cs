using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.Apply.WorkHistory;

public class GetJobsApiRequest(Guid ApplicationId, Guid CandidateId) : IGetApiRequest
{
    public string GetUrl => $"applications/{ApplicationId}/jobs?candidateId={CandidateId}";
}

public class GetJobsApiResponse
{
    public List<Job> Jobs { get; set; } = null!;

    public class Job
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