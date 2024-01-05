using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, List<string> levels, int distance, string searchTerm, int pageNumber, int pageSize, VacancySort sort)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, levels, distance, searchTerm, pageNumber, pageSize, sort);

        actual.GetUrl.Should().Be($"searchapprenticeships/searchResults?location={location}&routeIds={string.Join("&routeIds=",routes)}&levelIds={string.Join("&levelIds=",levels)}&distance={distance}&WhatSearchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}&sort={sort}");
    }

    [Test]
    public void Then_With_No_Params_The_Url_Is_Correctly_Built()
    {
        var actual = new GetSearchResultsApiRequest(null, null, null, null, null, null, null, VacancySort.DistanceAsc);

        actual.GetUrl.Should().Be("searchapprenticeships/searchResults?location=&routeIds=&levelIds=&distance=&WhatSearchTerm=&pageNumber=&pageSize=&sort=DistanceAsc");
    }
}