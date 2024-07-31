using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
public class GetInterviewAdjustmentsQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetInterviewAdjustmentsQuery, GetInterviewAdjustmentsQueryResult>
{
    public async Task<GetInterviewAdjustmentsQueryResult> Handle(GetInterviewAdjustmentsQuery query, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetInterviewAdjustmentsApiResponse>(new GetInterviewAdjustmentsApiRequest(query.ApplicationId, query.CandidateId));

        if (response == null) throw new ResourceNotFoundException();

        return response;
    }
}
