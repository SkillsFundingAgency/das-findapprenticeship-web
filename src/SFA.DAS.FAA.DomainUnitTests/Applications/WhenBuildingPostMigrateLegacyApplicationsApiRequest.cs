using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Applications.MigrateApplications;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications
{
    public class WhenBuildingPostMigrateLegacyApplicationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId, PostMigrateLegacyApplicationsApiRequest.PostMigrateLegacyApplicationsRequestData data)
        {
            var actual = new PostMigrateLegacyApplicationsApiRequest(candidateId, data);

            actual.PostUrl.Should().Be($"applications/{candidateId}/migrate");
        }
    }
}
