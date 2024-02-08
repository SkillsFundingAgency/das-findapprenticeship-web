using Microsoft.AspNetCore.Mvc;
namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddTrainingCourseViewModel : TrainingCourseViewModelBase
{
    [FromRoute]
    public Guid ApplicationId { get; set; }
}

public class TrainingCourseViewModelBase
{
    public string? CourseName { get; set; }
    public string? YearAchieved { get; set; }
}