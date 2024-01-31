using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;

namespace SFA.DAS.FAA.Domain.UnitTests.UpdateApplication;
public class WhenBuildingUpdateTrainingCoursesApplicationApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
    Guid applicationId,
    Guid candidateId)
    {
        var actual = new UpdateTrainingCoursesApplicationApiRequest(applicationId, candidateId, null!);

        actual.PostUrl.Should().Be($"applications/{applicationId}/{candidateId}/training-courses");
    }
}
