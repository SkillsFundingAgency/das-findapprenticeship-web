using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Applications.GetSubmittedApplications;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications
{
    public class WhenBuildingGetSubmittedApplicationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId)
        {
            var actual = new GetSubmittedApplicationsApiRequest(candidateId);

            actual.GetUrl.Should().Be($"applications/submitted?candidateId={candidateId}");
        }
    }
}
