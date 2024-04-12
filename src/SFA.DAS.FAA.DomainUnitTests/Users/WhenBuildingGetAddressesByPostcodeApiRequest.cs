using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingGetAddressesByPostcodeApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_built(Guid candidateId, string postcode)
    {
        var actual = new GetAddressesByPostcodeApiRequest(candidateId, HttpUtility.UrlEncode(postcode));

        actual.GetUrl.Should().Be($"users/{candidateId}/select-address?postcode={postcode}");
    }
}
