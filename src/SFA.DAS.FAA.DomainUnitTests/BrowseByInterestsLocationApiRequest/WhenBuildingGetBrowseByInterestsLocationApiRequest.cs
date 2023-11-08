using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.BrowseByInterestsLocation;

namespace SFA.DAS.FAA.Domain.UnitTests.BrowseByInterestsLocationApiRequest;

public class WhenBuildingGetBrowseByInterestsLocationApiRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Sent_And_Encoded(string searchTerm)
    {
        searchTerm = searchTerm + "£$%@! " + searchTerm;
        
        var actual = new GetBrowseByInterestsLocationApiRequest(searchTerm);

        actual.GetUrl.Should()
            .Be($"searchapprenticeships/browsebyinterestslocation?locationSearchTerm={HttpUtility.UrlEncode(searchTerm)}");
    }
}