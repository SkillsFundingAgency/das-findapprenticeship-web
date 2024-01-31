

using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users
{
    public class WhenBuildingAddNameRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_built(string govIdentifier, UpdateNameApiRequestData data)
        {
            var actual = new UpdateNameApiRequest(govIdentifier, data);

            actual.PutUrl.Should().Be($"users/{govIdentifier}/add-details");
            ((UpdateNameApiRequestData)actual.Data).Should().BeEquivalentTo(data);
        }
    }
}
