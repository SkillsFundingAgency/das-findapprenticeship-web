using MediatR;
using SFA.DAS.FAA.Application.Commands.TrainingCourses.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse.DeleteTrainingCourseApiRequest;

namespace SFA.FAA.FindAnApprenticeship.Application.TrainingCourses.DeleteTrainingCourse
{
    public class DeleteTrainingCourseCommandHandler : IRequestHandler<DeleteTrainingCourseCommand, Unit>
    {
        private readonly IApiClient _apiClient;

        public DeleteTrainingCourseCommandHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<Unit> Handle(DeleteTrainingCourseCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteTrainingCourseApiRequest(command.ApplicationId, command.TrainingCourseId, new DeleteTrainingCourseApiRequestData
            {
                CandidateId = command.CandidateId,
            });

            await _apiClient.PostWithResponseCode(request);
            return Unit.Value;
        }
    }

}
