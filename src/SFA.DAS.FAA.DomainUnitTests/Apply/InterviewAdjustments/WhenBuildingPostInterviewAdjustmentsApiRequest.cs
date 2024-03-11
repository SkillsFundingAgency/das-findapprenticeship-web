using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.InterviewAdjustments;
public class WhenBuildingPostInterviewAdjustmentsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        PostInterviewAdjustmentsModel data)
    {
        var actual = new PostInterviewAdjustmentsApiRequest(applicationId, data);

        actual.PostUrl.Should().Be($"applications/{applicationId}/interviewadjustments");
    }
}
