using MediatR;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;
public class GetDeleteTrainingCourseQueryHandler(IApiClient apiClient)
    : IRequestHandler<GetDeleteTrainingCourseQuery, GetDeleteTrainingCourseQueryResult>
{
    public async Task<GetDeleteTrainingCourseQueryResult> Handle(GetDeleteTrainingCourseQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetDeleteTrainingCourseApiResponse>(new GetDeleteTrainingCourseApiRequest(request.ApplicationId, request.CandidateId, request.TrainingCourseId));
        return GetDeleteTrainingCourseQueryResult.From(result);
    }
}
