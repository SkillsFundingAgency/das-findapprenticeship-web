using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, int distance, string searchTerm, int pageNumber, int pageSize)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, distance, searchTerm, pageNumber, pageSize);

        actual.GetUrl.Should().Be($"searchapprenticeships/searchResults?location={location}&routeIds={string.Join("&routeIds=", routes)}&distance={distance}&searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}");
    }

    [Test]
    public void Then_With_No_Params_The_Url_Is_Correctly_Built()
    {
        var actual = new GetSearchResultsApiRequest(null, null, null, null, null, null);

        actual.GetUrl.Should().Be("searchapprenticeships/searchResults?location=&routeIds=&distance=&searchTerm=&pageNumber=&pageSize=");
    }
}