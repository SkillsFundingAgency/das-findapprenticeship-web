using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Applications.MigrateData;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications
{
    public class WhenBuildingPostMigrateLegacyApplicationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId, PostMigrateDataTransferApiRequest.PostMigrateDataTransferApiRequestData data)
        {
            var actual = new PostMigrateDataTransferApiRequest(candidateId, data);

            actual.PostUrl.Should().Be($"user/{candidateId}/migrate");
        }
    }
}
