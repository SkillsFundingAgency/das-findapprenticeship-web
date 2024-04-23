using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.User;

namespace SFA.DAS.FAA.Domain.UnitTests.Users;
public class WhenBuildingPutCandidatePreferencesApiRequest
{
    [Test, AutoData]
    public void Then_Then_Request_Is_Built(Guid candidateId)
    {
        var actual = new UpsertCandidatePreferencesApiRequest(candidateId, new UpsertCandidatePreferencesData());

        actual.PostUrl.Should().Be($"users/{candidateId}/create-account/candidate-preferences");
    }
}
