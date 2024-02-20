using MediatR;

namespace SFA.DAS.FAA.Application.Commands.TrainingCourses.DeleteTrainingCourse
{
    public class DeleteTrainingCourseCommand : IRequest<Unit>
    {
        public Guid TrainingCourseId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
