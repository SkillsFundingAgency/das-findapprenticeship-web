using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetApplicationView;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications
{
    public class WhenBuildingGetApplicationViewApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId)
        {
            var actual = new GetApplicationViewApiRequest(applicationId, candidateId);

            actual.GetUrl.Should().Be($"applications/{applicationId}/{candidateId}/view");
        }
    }
}