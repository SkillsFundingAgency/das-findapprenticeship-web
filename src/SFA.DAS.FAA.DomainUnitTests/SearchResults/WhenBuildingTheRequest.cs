using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, int distance, string searchTerm)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, distance, searchTerm);

        actual.GetUrl.Should().Be($"searchapprenticeships/searchResults?location={location}&routes={string.Join("&routes=",routes)}&distance={distance}&searchTerm={searchTerm}");
    }

    [Test]
    public void Then_With_No_Params_The_Url_Is_Correctly_Built()
    {
        var actual = new GetSearchResultsApiRequest(null, null, null, null);

        actual.GetUrl.Should().Be("searchapprenticeships/searchResults?location=&routes=&distance=&searchTerm=");
    }
}