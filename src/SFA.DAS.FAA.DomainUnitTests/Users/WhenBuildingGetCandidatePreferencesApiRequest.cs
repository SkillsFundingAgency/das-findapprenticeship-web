using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingGetCandidatePreferencesApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_built(Guid candidateId)
    {
        var actual = new GetCandidatePreferencesApiRequest(candidateId);

        actual.GetUrl.Should().Be($"users/{candidateId}/create-account/candidate-preferences");
    }
}
