using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourse;
namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class AddTrainingCourseViewModel : TrainingCourseViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
    }

    public class EditTrainingCourseViewModel : TrainingCourseViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }

        [FromRoute]
        public Guid TrainingCourseId { get; set; }

        public static implicit operator EditTrainingCourseViewModel(GetTrainingCourseQueryResult source)
        {
            return new EditTrainingCourseViewModel
            {
                ApplicationId = source.ApplicationId,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved.ToString()
            };
        }
    }

    public class TrainingCourseViewModelBase
    {
        public string? CourseName { get; set; }
        public string? YearAchieved { get; set; }
    }
}