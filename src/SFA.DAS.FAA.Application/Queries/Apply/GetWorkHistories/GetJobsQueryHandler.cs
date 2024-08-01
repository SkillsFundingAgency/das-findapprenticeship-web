using MediatR;
using SFA.DAS.FAA.Domain.Apply.WorkHistory;
using SFA.DAS.FAA.Domain.Exceptions;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;

public record GetJobsQueryHandler(IApiClient ApiClient) : IRequestHandler<GetJobsQuery, GetJobsQueryResult>
{
    public async Task<GetJobsQueryResult> Handle(GetJobsQuery request, CancellationToken cancellationToken)
    {
        var response = await ApiClient.Get<GetJobsApiResponse>(
            new GetJobsApiRequest(request.ApplicationId, request.CandidateId));

        if (response == null) throw new ResourceNotFoundException();

        return response;
    }
}