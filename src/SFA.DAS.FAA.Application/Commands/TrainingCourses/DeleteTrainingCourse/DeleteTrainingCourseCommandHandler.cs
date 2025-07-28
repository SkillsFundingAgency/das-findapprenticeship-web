using MediatR;
using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;
using SFA.DAS.FAA.Domain.Interfaces;
using static SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse.DeleteTrainingCourseApiRequest;

namespace SFA.DAS.FAA.Application.Commands.TrainingCourses.DeleteTrainingCourse
{
    public class DeleteTrainingCourseCommandHandler(IApiClient apiClient)
        : IRequestHandler<DeleteTrainingCourseCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTrainingCourseCommand command, CancellationToken cancellationToken)
        {
            var request = new DeleteTrainingCourseApiRequest(command.ApplicationId, command.TrainingCourseId, new DeleteTrainingCourseApiRequestData
            {
                CandidateId = command.CandidateId,
            });

            await apiClient.Post(request);
            return Unit.Value;
        }
    }

}
