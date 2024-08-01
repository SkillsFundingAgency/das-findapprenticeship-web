using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetApplicationView;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationView
{
    public record GetApplicationViewQueryHandler(IApiClient ApiClient)
        : IRequestHandler<GetApplicationViewQuery, GetApplicationViewQueryResult>
    {
        public async Task<GetApplicationViewQueryResult> Handle(GetApplicationViewQuery query, CancellationToken cancellationToken)
        {
            var response = await ApiClient.Get<GetApplicationViewApiResponse>(new GetApplicationViewApiRequest(query.ApplicationId, query.CandidateId));
            if (response == null) throw new ResourceNotFoundException();
            return response;
        }
    }
}