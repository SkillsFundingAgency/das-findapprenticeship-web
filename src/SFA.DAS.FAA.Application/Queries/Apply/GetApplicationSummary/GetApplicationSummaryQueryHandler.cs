using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSummary;

public class GetApplicationSummaryQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetApplicationSummaryQuery, GetApplicationSummaryQueryResult>
{
    public async Task<GetApplicationSummaryQueryResult> Handle(GetApplicationSummaryQuery query, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetApplicationSummaryApiResponse>(new GetApplicationSummaryApiRequest(query.ApplicationId, query.CandidateId));
        return response;
    }
}