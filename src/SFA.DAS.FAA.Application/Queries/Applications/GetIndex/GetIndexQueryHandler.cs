using MediatR;
using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetIndex;

public class GetIndexQueryHandler(IApiClient apiClient) : IRequestHandler<GetIndexQuery, GetIndexQueryResult>
{
    public async Task<GetIndexQueryResult> Handle(GetIndexQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetApplicationsApiResponse>(
            new GetApplicationsApiRequest(request.CandidateId, request.Status));

        return response;
    }
}