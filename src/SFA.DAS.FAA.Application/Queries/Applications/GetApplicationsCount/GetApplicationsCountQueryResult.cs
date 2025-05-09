using SFA.DAS.FAA.Domain.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.Models;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount
{
    public record GetApplicationsCountQueryResult
    {
        public ApplicationStatusCount Stats { get; init; }

        public static implicit operator GetApplicationsCountQueryResult(GetApplicationsCountApiResponse source)
        {
            return new GetApplicationsCountQueryResult
            {
                Stats = new ApplicationStatusCount(
                    source.ApplicationIds,
                    source.Count,
                    (ApplicationStatus)Enum.Parse(typeof(ApplicationStatus), source.Status, ignoreCase: true))
            };
        }
    }
}