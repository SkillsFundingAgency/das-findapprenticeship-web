using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSubmitted;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.ApplicationSubmitted;
public class WhenBuildingApplicationSubmittedApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new GetApplicationSubmittedApiRequest(applicationId, candidateId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/submitted?candidateId={candidateId}");
    }
}
