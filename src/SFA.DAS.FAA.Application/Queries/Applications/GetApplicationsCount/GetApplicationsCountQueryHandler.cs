using MediatR;
using SFA.DAS.FAA.Domain.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetApplicationsCount
{
    public class GetApplicationsCountQueryHandler(IApiClient apiClient)
        : IRequestHandler<GetApplicationsCountQuery, GetApplicationsCountQueryResult>
    {
        public async Task<GetApplicationsCountQueryResult> Handle(GetApplicationsCountQuery request, CancellationToken cancellationToken)
        {
            var response = await apiClient.Get<GetApplicationsCountApiResponse>(
                new GetApplicationsCountApiRequest(request.CandidateId, request.Status));

            return response;
        }
    }
}
