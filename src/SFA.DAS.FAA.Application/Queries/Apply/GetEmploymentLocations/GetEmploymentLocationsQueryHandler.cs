using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocations;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocations;

public class GetEmploymentLocationsQueryHandler(IApiClient apiClient) : IRequestHandler<GetEmploymentLocationsQuery, GetEmploymentLocationsQueryResult>
{
    public async Task<GetEmploymentLocationsQueryResult> Handle(GetEmploymentLocationsQuery request, CancellationToken cancellationToken)
    {
        return await apiClient.Get<GetEmploymentLocationsApiResponse>(new GetEmploymentLocationsApiRequest(request.ApplicationId, request.CandidateId));
    }
}