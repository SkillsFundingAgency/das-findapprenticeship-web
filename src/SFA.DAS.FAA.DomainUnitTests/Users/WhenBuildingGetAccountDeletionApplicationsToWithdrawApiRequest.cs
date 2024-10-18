using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;


namespace SFA.DAS.FAA.Domain.UnitTests.Users
{
    public class WhenBuildingGetAccountDeletionApplicationsToWithdrawApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId)
        {
            var actual = new GetAccountDeletionApplicationsToWithdrawApiRequest(candidateId);

            actual.GetUrl.Should().Be($"users/{candidateId}/account-deletion");
        }
    }
}
