using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SavedSearches;

namespace SFA.DAS.FAA.Domain.UnitTests.SavedSearches;

public class WhenBuildingPostSavedSearchUnsubscribeApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Build_With_The_Search_Id(Guid searchId)
    {
        var actual = new PostSavedSearchUnsubscribeApiRequest(searchId);
        
        actual.PostUrl.Should().Be($"saved-searches/unsubscribe");
        ((PostSavedSearchUnsubscribeApiRequestData)actual.Data).SavedSearchId.Should().Be(searchId);
    }
}