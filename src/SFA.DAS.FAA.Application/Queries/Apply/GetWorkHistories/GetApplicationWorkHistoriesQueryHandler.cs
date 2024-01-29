using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public class GetApplicationWorkHistoriesQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetApplicationWorkHistoriesQuery, GetApplicationWorkHistoriesQueryResult>
{
    public async Task<GetApplicationWorkHistoriesQueryResult> Handle(GetApplicationWorkHistoriesQuery request, CancellationToken cancellationToken)
    {
        var workHistories = await apiClient.Get<List<WorkHistory>>(new GetApplicationWorkHistoriesApiRequest(request.CandidateId, request.ApplicationId));

        return new GetApplicationWorkHistoriesQueryResult
        {
            WorkHistories = workHistories
        };
    }
}