using AutoFixture.NUnit3;
using NUnit.Framework;

namespace SFA.DAS.FAA.Domain.UnitTests.SavedSearches;

public class WhenBuildingPostSavedSearchUnsubscribeApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Build_With_The_Search_Id(Guid searchId)
    {
        var actual = new PostSavedSearchUnsubscribeApiRequest(searchId);
        
        actual.PostUrl.Should().Be($"saved-searches/{searchId}/unsubscribe");
    }
}