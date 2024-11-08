using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;
using static SFA.DAS.FAA.Domain.SearchResults.PostSaveSearchApiRequest;

namespace SFA.DAS.FAA.Domain.UnitTests.SavedSearch
{
    [TestFixture]
    public class WhenBuildingPostSaveSearchApiRequest
    {
        [Test, AutoData]
        public void Then_Then_Request_Is_Built(
            Guid candidateId,
            Guid id,
            PostSaveSearchApiRequestData payload)
        {
            var actual = new PostSaveSearchApiRequest(candidateId, id, payload);

            actual.PostUrl.Should().Be($"searchapprenticeships/saved-search?candidateId={candidateId}&id={id}");
        }
    }
}
