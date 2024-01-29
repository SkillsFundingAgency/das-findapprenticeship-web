

using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users
{
    public class WhenBuildingAddNameRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_built(string govIdentifier, string firstName, string lastName,string email)
        {
            var actual = new UpdateNameApiRequest(firstName, lastName, govIdentifier, email);

            actual.PutUrl.Should().Be($"/users/{govIdentifier}/add-details");
        }
    }
}
