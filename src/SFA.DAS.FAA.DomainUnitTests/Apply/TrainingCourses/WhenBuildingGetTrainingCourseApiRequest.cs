using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetTrainingCourse;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.TrainingCourses;
public class WhenBuildingGetTrainingCourseApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
    Guid applicationId,
    Guid candidateId,
    Guid trainingCourseId)
    {
        var actual = new GetTrainingCourseApiRequest(applicationId, candidateId, trainingCourseId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/trainingcourses/{trainingCourseId}?candidateId={candidateId}");
    }
}
