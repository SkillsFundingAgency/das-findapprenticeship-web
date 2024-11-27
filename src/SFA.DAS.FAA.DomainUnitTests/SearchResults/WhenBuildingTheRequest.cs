using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, List<string> levels, int distance, string searchTerm, int pageNumber, int pageSize, VacancySort sort, WageType skipWageType, bool disabilityConfident, string candidateId)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, levels, distance, searchTerm, pageNumber, pageSize, sort, skipWageType, disabilityConfident, candidateId);

        actual.GetUrl.Should().Be($"searchapprenticeships/searchResults?location={location}&distance={distance}&searchTerm={searchTerm}&pageNumber={pageNumber}&pageSize={pageSize}&sort={sort}&disabilityConfident={disabilityConfident}&candidateId={candidateId}&skipWageType={skipWageType}&routeIds={string.Join("&routeIds=",routes)}&levelIds={string.Join("&levelIds=", levels)}");
    }

    [Test]
    public void Then_With_No_Params_The_Url_Is_Correctly_Built()
    {
        var actual = new GetSearchResultsApiRequest(null, null, null, null, null, null, null, VacancySort.DistanceAsc, null, false, null);

        actual.GetUrl.Should().Be("searchapprenticeships/searchResults?location=&distance=&searchTerm=&pageNumber=&pageSize=&sort=DistanceAsc&disabilityConfident=False&candidateId=&skipWageType=");
    }
}