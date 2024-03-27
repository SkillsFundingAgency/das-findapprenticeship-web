using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.DisabilityConfident;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.DisabilityConfident;

public class WhenBuildingGetDisabilityConfidentDetailsApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(
        Guid applicationId,
        Guid candidateId)
    {
        var actual = new GetDisabilityConfidentDetailsApiRequest(applicationId, candidateId);

        actual.GetUrl.Should().Be($"applications/{applicationId}/disability-confident/details?candidateId={candidateId}");
    }
}