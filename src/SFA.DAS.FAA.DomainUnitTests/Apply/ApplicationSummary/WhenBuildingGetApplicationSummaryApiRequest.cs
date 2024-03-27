using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Apply.GetApplicationSummary;

namespace SFA.DAS.FAA.Domain.UnitTests.Apply.ApplicationSummary
{
    public class WhenBuildingGetApplicationSummaryApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid applicationId,
            Guid candidateId)
        {
            var actual = new GetApplicationSummaryApiRequest(applicationId, candidateId);

            actual.GetUrl.Should().Be($"applications/{applicationId}/{candidateId}/summary");
        }
    }
}