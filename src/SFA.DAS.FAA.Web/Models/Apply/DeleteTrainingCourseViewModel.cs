using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetDeleteTrainingCourse;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class DeleteTrainingCourseViewModel : TrainingCourseViewModelBase
    {
        [FromRoute]
        public Guid ApplicationId { get; set; }
        [FromRoute]
        public Guid TrainingCourseId { get; set; }

        public static implicit operator DeleteTrainingCourseViewModel(GetDeleteTrainingCourseQueryResult source)
        {
            return new DeleteTrainingCourseViewModel
            {
                ApplicationId = source.ApplicationId,
                CourseName = source.CourseName,
                YearAchieved = source.YearAchieved.ToString()
            };
        }

    }
}
