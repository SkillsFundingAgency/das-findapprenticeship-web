using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsQueryHandler : IRequestHandler<GetInterviewAdjustmentsQuery, GetInterviewAdjustmentsQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetInterviewAdjustmentsQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetInterviewAdjustmentsQueryResult> Handle(GetInterviewAdjustmentsQuery query, CancellationToken cancellationToken)
    {
        var response = await _apiClient.Get<GetInterviewAdjustmentsApiResponse>(new GetInterviewAdjustmentsApiRequest(query.ApplicationId, query.CandidateId));
        return response;
    }
}
