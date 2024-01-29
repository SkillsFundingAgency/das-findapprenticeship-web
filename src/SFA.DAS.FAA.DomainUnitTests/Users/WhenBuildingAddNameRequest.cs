

using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users
{
    public class WhenBuildingAddNameRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_built(Guid candidateId, string firstName, string lastName)
        {
            var actual = new UpdateNameApiRequest(firstName, lastName, candidateId);

            actual.PutUrl.Should().Be($"/users/{candidateId}/add-details");
        }
    }
}
