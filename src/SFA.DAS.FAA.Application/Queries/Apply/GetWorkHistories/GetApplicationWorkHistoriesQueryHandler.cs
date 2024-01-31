using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public record GetApplicationWorkHistoriesQueryHandler(IApiClient ApiClient)
    : IRequestHandler<GetApplicationWorkHistoriesQuery, GetApplicationWorkHistoriesQueryResult>
{
    public async Task<GetApplicationWorkHistoriesQueryResult> Handle(GetApplicationWorkHistoriesQuery request, CancellationToken cancellationToken)
    {
        var workHistories = await ApiClient.Get<List<WorkHistory>>(
            new GetApplicationWorkHistoriesApiRequest(request.ApplicationId, request.CandidateId));

        return new GetApplicationWorkHistoriesQueryResult
        {
            WorkHistories = workHistories
        };
    }
}