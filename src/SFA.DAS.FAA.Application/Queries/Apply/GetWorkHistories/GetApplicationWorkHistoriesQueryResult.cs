using SFA.DAS.FAA.Domain.Apply.WorkHistory;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public class GetApplicationWorkHistoriesQueryResult
{
    public List<WorkHistory> WorkHistories { get; set; } = [];
}