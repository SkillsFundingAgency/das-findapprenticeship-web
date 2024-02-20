using MediatR;

namespace SFA.DAS.FAA.Application.Commands.TrainingCourses.UpdateTrainingCourse;
public class UpdateTrainingCourseCommand : IRequest
{
    public Guid TrainingCourseId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }
}
