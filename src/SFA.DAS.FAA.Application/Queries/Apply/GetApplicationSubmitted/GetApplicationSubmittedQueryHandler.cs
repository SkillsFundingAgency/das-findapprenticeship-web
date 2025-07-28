using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetApplicationSubmitted;
public class GetApplicationSubmittedQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetApplicationSubmittedQuery, GetApplicationSubmittedQueryResponse>
{
    public async Task<GetApplicationSubmittedQueryResponse> Handle(GetApplicationSubmittedQuery query, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetApplicationSubmittedApiResponse>(new GetApplicationSubmittedApiRequest(query.ApplicationId, query.CandidateId));

        if (response == null) throw new ResourceNotFoundException();

        return response;
    }
}
