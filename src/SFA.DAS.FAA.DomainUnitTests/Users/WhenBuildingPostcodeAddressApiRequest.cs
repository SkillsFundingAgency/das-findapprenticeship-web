using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingPostcodeAddressApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_built(string postcode)
    {
        var actual = new GetCandidatePostcodeAddressApiRequest(postcode);

        actual.GetUrl.Should().Be($"users/create-account/postcode-address?postcode={postcode}");
    }
}
