using SFA.DAS.FAA.Application.Queries.Apply.GetTrainingCourses;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.UnitTests.Models.Apply;

public class TrainingCoursesViewModelTests
{
    [Test, MoqAutoData]
    public void Map_Returns_Expected_Result(
            GetTrainingCoursesQueryResult.TrainingCourse source)
    {
        var result = (TrainingCoursesViewModel.TrainingCourse)source;

        using (new AssertionScope())
        {
            result.Id.Should().Be(source.Id);
            result.CourseName.Should().Be(source.CourseName);
            result.YearAchieved.Should().Be(source.YearAchieved);
        }
    }
}