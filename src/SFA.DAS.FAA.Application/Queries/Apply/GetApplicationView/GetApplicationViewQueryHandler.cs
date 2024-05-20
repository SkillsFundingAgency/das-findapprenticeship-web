using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView
{
    public record GetApplicationViewQueryHandler(IApiClient ApiClient)
        : IRequestHandler<GetApplicationViewQuery, GetApplicationViewQueryResult>
    {
        public async Task<GetApplicationViewQueryResult> Handle(GetApplicationViewQuery query, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetApplicationViewApiResponse>(new GetApplicationViewApiRequest(query.ApplicationId, query.CandidateId));
            return response;
        }
    }
}