using MediatR;
using SFA.DAS.FAA.Domain.Apply.InterviewAdjustments;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.InterviewAdjustments;

public record GetInterviewAdjustmentsQueryHandler(IApiClient ApiClient) : IRequestHandler<GetInterviewAdjustmentsQuery, GetInterviewAdjustmentsQueryResult>
{
    public async Task<GetInterviewAdjustmentsQueryResult> Handle(GetInterviewAdjustmentsQuery request, CancellationToken cancellationToken)
    {
        var response = await ApiClient.Get<GetInterviewAdjustmentsApiResponse>(
            new GetInterviewAdjustmentsApiRequest(request.ApplicationId, request.CandidateId));

        return response;
    }
}