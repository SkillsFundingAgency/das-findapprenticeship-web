using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.AddTrainingCourse;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.TrainingCourses;
public class WhenBuildingPostTrainingCourseApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
    Guid applicationId,
    PostTrainingCourseApiRequest.PostTrainingCourseApiRequestData data)
    {
        var actual = new PostTrainingCourseApiRequest(applicationId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/trainingcourses");
    }
}
