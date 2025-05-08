using SFA.DAS.FAA.Domain.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQueryResult
    {
        public List<ApplicationStats> Stats { get; init; } = [];

        public static implicit operator GetApplicationsCountQueryResult(GetApplicationsCountApiResponse source)
        {
            return new GetApplicationsCountQueryResult
            {
                Stats = source.Stats.Select(x => (ApplicationStats)x).ToList()
            };
        }

        public record ApplicationStats
        {
            public ApplicationStatus Status { get; set; }
            public int Count { get; set; }

            public static implicit operator ApplicationStats(GetApplicationsCountApiResponse.ApplicationStats source)
            {
                return new ApplicationStats
                {
                    Status = (ApplicationStatus)Enum.Parse(typeof(ApplicationStatus), source.Status, ignoreCase: true),
                    Count = source.Count
                };
            }
        }
    }
}
