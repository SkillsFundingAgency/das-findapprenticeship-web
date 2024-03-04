using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetInterviewAdjustments;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.InterviewAdjustments;
public class WhenBuildingGetInterviewAdjustmentsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new GetInterviewAdjustmentsApiRequest(applicationId, candidateId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/interview-adjustments?candidateId={candidateId}");
    }
}
