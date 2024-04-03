using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingGetCandidateDateOfBirthApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_built(string govUkIdentifier)
    {
        var actual = new GetCandidateDateOfBirthApiRequest(govUkIdentifier);

        actual.GetUrl.Should().Be($"users/{govUkIdentifier}/date-of-birth");
    }
}
