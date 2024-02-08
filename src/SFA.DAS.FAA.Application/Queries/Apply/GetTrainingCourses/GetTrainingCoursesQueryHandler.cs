using MediatR;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourses;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
public class GetTrainingCoursesQueryHandler(IApiClient ApiClient) : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
{
    public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
    {
        var response = await ApiClient.Get<GetTrainingCoursesApiResponse>(
            new GetTrainingCoursesApiRequest(request.ApplicationId, request.CandidateId));

        return response;
    }
}
