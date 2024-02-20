using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.TrainingCourses.UpdateTrainingCourse;
public class UpdateTrainingCourseCommandHandler(IApiClient ApiClient) : IRequestHandler<UpdateTrainingCourseCommand>
{
    public async Task Handle(UpdateTrainingCourseCommand request, CancellationToken cancellationToken)
    {
        var data = new PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData
        {
            CandidateId = request.CandidateId,
            CourseName = request.CourseName,
            YearAchieved = request.YearAchieved,
        };

        var apiRequest = new PostUpdateTrainingCourseApiRequest(request.ApplicationId, request.TrainingCourseId, data);

        await ApiClient.PostWithResponseCode(apiRequest);
    }
}
