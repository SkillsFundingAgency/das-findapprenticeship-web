using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Domain.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Domain.UnitTests.SearchResults;

public class WhenBuildingTheRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Constructed(string location, List<string> routes, int distance)
    {
        var actual = new GetSearchResultsApiRequest(location, routes, distance);

        actual.GetUrl.Should().Be($"vacancies?location={location}?routes={routes}?distance={distance}");
    }
}