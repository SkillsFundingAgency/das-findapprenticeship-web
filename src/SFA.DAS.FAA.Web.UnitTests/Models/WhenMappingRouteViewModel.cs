using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Web.Models.SearchResults;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenMappingRouteViewModel
{
    [TestCase(1, "A Name", "A Name")]
    [TestCase(2, "A Different Name", "A Different Name")]
    [TestCase(5, "This value will be mapped", "Construction and building")]
    [TestCase(14, "This value will also be mapped", "Sales and marketing")]
    public void Then_The_Route_Is_Mapped_With_The_Correct_Name(int routeId, string routeName, string expectedRouteName)
    {
        // arrange
        var routeResponse = new RouteResponse
        {
            Id = routeId,
            Name = routeName
        };

        // act
        var result = RouteViewModel.ToViewModel(routeResponse);

        // assert
        result.Id.Should().Be(routeId);
        result.Name.Should().Be(expectedRouteName);
        result.Selected.Should().BeFalse();
    }
}