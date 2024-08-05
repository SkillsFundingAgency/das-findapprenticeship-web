using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Candidates;

namespace SFA.DAS.FAA.Domain.UnitTests.Candidates
{
    public class WhenBuildingUpdateCandidateStatusApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            string govIdentifier,
            UpdateCandidateStatusApiRequest.UpdateCandidateStatusApiRequestData data)
        {
            var actual = new UpdateCandidateStatusApiRequest(govIdentifier, data);

            actual.PostUrl.Should().Be($"candidates/{govIdentifier}/status");
        }
    }
}
