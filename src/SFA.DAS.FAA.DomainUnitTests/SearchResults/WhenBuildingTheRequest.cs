using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, int distance, string searchTerm, int pageNumber, int pageSize, string? sort)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, distance, searchTerm, pageNumber, pageSize, sort);

        actual.GetUrl.Should().Be($"searchapprenticeships/searchResults?location={location}&routes={string.Join("&routes=",routes)}&distance={distance}&searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}&sort={sort}");
    }

    [Test]
    public void Then_With_No_Params_The_Url_Is_Correctly_Built()
    {
        var actual = new GetSearchResultsApiRequest(null, null, null, null, null, null, null);

        actual.GetUrl.Should().Be("searchapprenticeships/searchResults?location=&routes=&distance=&searchTerm=&pageNumber=&pageSize=&sort=");
    }
}