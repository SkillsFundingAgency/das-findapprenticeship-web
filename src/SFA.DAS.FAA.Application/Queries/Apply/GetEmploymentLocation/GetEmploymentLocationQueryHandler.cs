using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetEmploymentLocation;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetEmploymentLocation;

public class GetEmploymentLocationQueryHandler(IApiClient apiClient) : IRequestHandler<GetEmploymentLocationQuery, GetEmploymentLocationQueryResult>
{
    public async Task<GetEmploymentLocationQueryResult> Handle(GetEmploymentLocationQuery request, CancellationToken cancellationToken)
    {
        return await apiClient.Get<GetEmploymentLocationApiResponse>(new GetEmploymentLocationApiRequest(request.ApplicationId, request.CandidateId));
    }
}