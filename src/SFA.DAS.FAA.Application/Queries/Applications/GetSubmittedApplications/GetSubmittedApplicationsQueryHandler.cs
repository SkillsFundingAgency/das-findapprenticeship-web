using MediatR;
using SFA.DAS.FAA.Domain.Applications.GetSubmittedApplications;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Applications.GetSubmittedApplications
{
    public record GetSubmittedApplicationsQueryHandler(IApiClient ApiClient) : IRequestHandler<GetSubmittedApplicationsQuery, GetSubmittedApplicationsQueryResult>
    {
        public async Task<GetSubmittedApplicationsQueryResult> Handle(GetSubmittedApplicationsQuery request, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetSubmittedApplicationsApiResponse>(
                new GetSubmittedApplicationsApiRequest(request.CandidateId));

            return response;
        }
    }
}