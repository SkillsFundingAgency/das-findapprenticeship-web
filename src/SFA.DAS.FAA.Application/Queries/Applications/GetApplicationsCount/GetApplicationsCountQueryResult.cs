using SFA.DAS.FAA.Domain.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQueryResult
    {
        public List<ApplicationStatusCount> Stats { get; init; } = [];

        public static implicit operator GetApplicationsCountQueryResult(GetApplicationsCountApiResponse source)
        {
            return new GetApplicationsCountQueryResult
            {
                Stats = source.Stats.Count > 0 
                    ? source.Stats.Select(x => new ApplicationStatusCount(
                    x.ApplicationIds,
                    x.Count,
                    (ApplicationStatus)Enum.Parse(typeof(ApplicationStatus), x.Status, ignoreCase: true))).ToList()
                    : [],
            };
        }
    }
}
