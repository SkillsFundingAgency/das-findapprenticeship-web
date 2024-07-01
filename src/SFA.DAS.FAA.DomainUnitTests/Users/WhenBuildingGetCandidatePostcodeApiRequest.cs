using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;

public class WhenBuildingGetCandidatePostcodeApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Constructed(Guid candidateId)
    {
        var actual = new GetCandidatePostcodeApiRequest(candidateId);

        actual.GetUrl.Should().Be($"users/{candidateId}/create-account/postcode");
    }
}