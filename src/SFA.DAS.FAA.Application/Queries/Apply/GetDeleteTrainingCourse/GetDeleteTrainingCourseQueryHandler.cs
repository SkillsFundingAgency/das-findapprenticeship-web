using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;
public class GetDeleteTrainingCourseQueryHandler : IRequestHandler<GetDeleteTrainingCourseQuery, GetDeleteTrainingCourseQueryResult>
{
    private readonly IApiClient _apiClient;

    public GetDeleteTrainingCourseQueryHandler(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<GetDeleteTrainingCourseQueryResult> Handle(GetDeleteTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        var result = await _apiClient.Get<GetDeleteTrainingCourseApiResponse>(new GetDeleteTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));
        return GetDeleteTrainingCourseQueryResult.From(result);
    }
}
