using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingDateOfBirthApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_built(Guid candidateId, string email, DateTime dob)
    {
        var data = new UpdateDateOfBirthRequestData() 
        { 
            DateOfBirth = dob,
            Email = email
        };
        var actual = new UpdateDateOfBirthApiRequest(candidateId, data);

        actual.PostUrl.Should().Be($"users/{candidateId}/date-of-birth");

        ((UpdateDateOfBirthRequestData)actual.Data).Should().BeEquivalentTo(data);
    }
}
