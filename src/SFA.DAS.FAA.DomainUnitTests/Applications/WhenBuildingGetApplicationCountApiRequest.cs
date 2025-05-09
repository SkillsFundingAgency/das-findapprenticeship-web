using AutoFixture.NUnit3;
using SFA.DAS.FAA.Domain.Applications.GetApplicationsCount;
using SFA.DAS.FAA.Domain.Enums;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications
{
    [TestFixture]
    public class WhenBuildingGetApplicationCountApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid candidateId,
            ApplicationStatus status)
        {
            var actual = new GetApplicationsCountApiRequest(candidateId, status);
            actual.GetUrl.Should().Be($"applications/count?candidateId={candidateId}&status={status}");
        }
    }
}