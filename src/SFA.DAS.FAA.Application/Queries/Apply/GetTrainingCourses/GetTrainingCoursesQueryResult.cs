using SFA.DAS.FAA.Domain.Apply.GetTrainingCourses;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
public record GetTrainingCoursesQueryResult
{
    public bool? IsSectionCompleted { get; set; }
    public List<TrainingCourse> TrainingCourses { get; set; } = null!;

    public class TrainingCourse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }

        public static implicit operator TrainingCourse(GetTrainingCoursesApiResponse.TrainingCourse source)
        {
            return new TrainingCourse
            {
                Id = source.Id,
                ApplicationId = source.ApplicationId,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved
            };
        }
    }

    public static implicit operator GetTrainingCoursesQueryResult(GetTrainingCoursesApiResponse source)
    {
        return new GetTrainingCoursesQueryResult
        {
            IsSectionCompleted = source.IsSectionCompleted,
            TrainingCourses = source.TrainingCourses.Select(x => (TrainingCourse)x).ToList()
        };
    }
}
