using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;

namespace SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;
public class GetDeleteTrainingCourseQueryResult
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string CourseName { get; set; }
    public int YearAchieved { get; set; }

    public static GetDeleteTrainingCourseQueryResult From(GetDeleteTrainingCourseApiResponse source)
    {
        return source is null
            ? null
            : new GetDeleteTrainingCourseQueryResult
            {
                Id = source.Id,
                ApplicationId = source.ApplicationId,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved
            };
    }
}