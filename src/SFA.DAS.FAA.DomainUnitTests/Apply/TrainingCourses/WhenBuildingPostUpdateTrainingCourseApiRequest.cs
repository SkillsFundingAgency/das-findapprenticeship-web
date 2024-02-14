using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.UpdateTrainingCourse;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.TrainingCourses;
public class WhenBuildingPostUpdateTrainingCourseApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid trainingCourseId,
        PostUpdateTrainingCourseApiRequest.PostUpdateTrainingCourseRequestData data)
    {
        var actual = new PostUpdateTrainingCourseApiRequest(applicationId, trainingCourseId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/trainingcourses/{trainingCourseId}");
    }
}