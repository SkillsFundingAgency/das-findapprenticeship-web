using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
public class GetDeleteTrainingCourseQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }

    public static implicit operator GetDeleteTrainingCourseQueryResult(GetDeleteTrainingCourseApiResponse source)
    {
        if (source == null)
            return null;

        return new GetDeleteTrainingCourseQueryResult
        {
            Id = source.Id,
            ApplicationId = source.ApplicationId,
            CourseName = source.CourseName,
            YearAchieved = source.YearAchieved
        };
    }
}