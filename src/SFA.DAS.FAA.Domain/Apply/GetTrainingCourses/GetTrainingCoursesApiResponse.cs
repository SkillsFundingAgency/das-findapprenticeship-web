namespace SFA.DAS.FAA.Domain.Apply.GetTrainingCourses;
public class GetTrainingCoursesApiResponse
{
    public List<TrainingCourse> TrainingCourses { get; set; } = null!;

    public class TrainingCourse
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public string CourseName { get; set; }
        public int YearAchieved { get; set; }
    }
}
