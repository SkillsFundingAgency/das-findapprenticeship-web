using MediatR;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;
using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Application.Commands.UpdateApplication.TrainingCourses;
public class UpdateTrainingCoursesApplicationCommandHandler(IApiClient apiClient) : IRequestHandler<UpdateTrainingCoursesApplicationCommand, UpdateTrainingCoursesApplicationCommandResult>
{
    public async Task<UpdateTrainingCoursesApplicationCommandResult> Handle(UpdateTrainingCoursesApplicationCommand request, CancellationToken cancellationToken)
    {
        var postRequest = new UpdateTrainingCoursesApplicationApiRequest(
            request.ApplicationId,
            request.CandidateId,
            new UpdateTrainingCoursesApplicationModel
            {
                TrainingCoursesSectionStatus = request.TrainingCoursesSectionStatus
            });

        var response = await apiClient.PostWithResponseCode<Domain.Apply.UpdateApplication.Application>(postRequest);

        return new UpdateTrainingCoursesApplicationCommandResult
        {
            Application = response
        };
    }
}
