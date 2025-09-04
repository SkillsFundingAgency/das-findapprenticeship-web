using System.Web;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Domain.SearchResults;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(
        string location, 
        List<string> routes, 
        List<string> levels, 
        int distance, 
        string searchTerm, 
        int pageNumber, 
        int pageSize, 
        VacancySort sort, 
        WageType skipWageType, 
        bool disabilityConfident, 
        string candidateId, 
        bool? excludeNational,
        List<ApprenticeshipTypes> apprenticeshipTypes)
    {
        // arrange
        var expectedUrl = $"searchapprenticeships/searchResults?" +
                          $"location={HttpUtility.UrlEncode(location + "&" + location)}" +
                          $"&distance={distance}" +
                          $"&searchTerm={HttpUtility.UrlEncode(searchTerm + "&" + searchTerm)}" +
                          $"&pageNumber={pageNumber}" +
                          $"&pageSize={pageSize}" +
                          $"&sort={sort}" +
                          $"&disabilityConfident={disabilityConfident}" +
                          $"&candidateId={candidateId}" +
                          $"&skipWageType={skipWageType}" +
                          $"&routeIds={string.Join("&routeIds=", routes)}" +
                          $"&levelIds={string.Join("&levelIds=", levels)}" +
                          $"&excludeNational={excludeNational}" +
                          $"&apprenticeshipTypes={string.Join("&apprenticeshipTypes=", apprenticeshipTypes)}";
        
        var actual = new GetSearchResultsApiRequest(
            location + "&" + location,
            routes,
            levels,
            distance,
            searchTerm + "&" + searchTerm,
            pageNumber,
            pageSize,
            sort,
            skipWageType,
            disabilityConfident,
            candidateId,
            excludeNational,
            apprenticeshipTypes);

        actual.GetUrl.Should().Be(expectedUrl);
    }

    [Test]
    public void Then_With_No_Params_The_Url_Is_Correctly_Built()
    {
        var actual = new GetSearchResultsApiRequest(null, null, null, null, null, null, null, VacancySort.DistanceAsc, null, false, null, null, null);

        actual.GetUrl.Should().Be("searchapprenticeships/searchResults?location=&distance=&searchTerm=&pageNumber=&pageSize=&sort=DistanceAsc&disabilityConfident=False&candidateId=&skipWageType=&excludeNational=");
    }
}