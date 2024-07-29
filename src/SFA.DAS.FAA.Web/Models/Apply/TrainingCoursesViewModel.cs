using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class TrainingCoursesViewModel
{
    public static readonly int MaximumItems = 25;

    [FromRoute]
    public required Guid ApplicationId { get; init; }
    public bool? DoYouWantToAddAnyTrainingCourses { get; set; }
    public string? BackLinkUrl { get; set; }
    public bool ShowTrainingCoursesAchieved { get; set; }
    public List<TrainingCourse>? TrainingCourses { get; set; }
    public bool? IsSectionComplete { get; set; }
    public bool MaximumItemsReached => TrainingCourses?.Count >= MaximumItems;

    public class TrainingCourse
    {
        public Guid Id { get; private init; }
        public string? CourseName { get; private init; }
        public int? YearAchieved { get; private init; }

        public static implicit operator TrainingCourse(GetTrainingCoursesQueryResult.TrainingCourse source)
        {
            return new TrainingCourse
            {
                Id = source.Id,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved
            };
        }
    }

}
