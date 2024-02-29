using SFA.DAS.FAA.Domain.Apply.WorkHistory;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public record GetJobsQueryResult
{
    public bool? IsSectionCompleted { get; set; }
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

        public static implicit operator Job(GetJobsApiResponse.Job source)
        {
            return new Job
            {
                Id = source.Id,
                Employer = source.Employer,
                JobTitle = source.JobTitle,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                ApplicationId = source.ApplicationId,
                Description = source.Description
            };
        }
    }

    public static implicit operator GetJobsQueryResult(GetJobsApiResponse source)
    {
        return new GetJobsQueryResult
        {
            IsSectionCompleted = source.IsSectionCompleted,
            Jobs = source.Jobs.Select(x => (Job)x).ToList()
        };
    }
}