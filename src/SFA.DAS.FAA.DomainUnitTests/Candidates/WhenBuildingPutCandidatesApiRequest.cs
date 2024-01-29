using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Candidates;

namespace SFA.DAS.FAA.Domain.UnitTests.Candidates;
public class WhenBuildingPutCandidatesApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string govIdentifier, PutCandidateApiRequestData data)
    {
        var actual = new PutCandidateApiRequest(govIdentifier, data);

        actual.PutUrl.Should().Be($"candidates/{govIdentifier}");
    }
}
