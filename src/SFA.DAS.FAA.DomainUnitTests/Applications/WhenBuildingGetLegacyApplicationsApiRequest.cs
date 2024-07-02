using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Applications
{
    public class WhenBuildingGetLegacyApplicationsApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            string emailAddress, Guid candidateId)
        {
            var actual = new GetMigrateDataTransferApiRequest(emailAddress, candidateId);

            actual.GetUrl.Should().Be($"users/migrate?emailAddress={HttpUtility.UrlEncode(emailAddress)}&candidateId={candidateId}");
        }
    }
}