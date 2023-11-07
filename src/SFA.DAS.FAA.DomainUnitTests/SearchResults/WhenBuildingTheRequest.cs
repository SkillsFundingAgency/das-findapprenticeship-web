using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, int distance)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, distance);

        actual.GetUrl.Should().Be($"vacancies?location={location}&routes={routes}&distance={distance}");
    }
}