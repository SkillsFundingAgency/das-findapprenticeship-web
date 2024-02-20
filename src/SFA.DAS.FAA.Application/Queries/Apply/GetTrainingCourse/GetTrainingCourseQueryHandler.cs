using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery, GetTrainingCourseQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetTrainingCourseQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetTrainingCourseQueryResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        var response = await _apiClient.Get<GetTrainingCourseApiResponse>(
            new GetTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));

        return response;
    }
}
