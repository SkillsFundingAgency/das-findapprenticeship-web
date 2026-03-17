using SFA.DAS.FAA.Domain.Apply.DeleteTrainingCourse;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.TrainingCourses;

public class WhenBuildingGetDeleteTrainingCourseApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
    Guid applicationId,
    Guid candidateId,
    Guid trainingCourseId)
    {
        var actual = new GetDeleteTrainingCourseApiRequest(applicationId, candidateId, trainingCourseId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/trainingcourses/{trainingCourseId}/delete?candidateId={candidateId}");
    }
}
