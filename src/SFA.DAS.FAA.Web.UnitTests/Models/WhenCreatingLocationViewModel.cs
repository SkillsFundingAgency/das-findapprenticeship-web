using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingLocationViewModel
{
    [Test, AutoData]
    public void Then_If_Selected_Routes_Is_Null_Dictionary_Returned()
    {
        //Act
        var actual = new LocationViewModel();

        //Assert
        actual.RouteData.Should().BeEquivalentTo(new Dictionary<string, string>());
    }
    [Test, AutoData]
    public void Then_If_No_Selected_Routes_Empty_Dictionary_Returned()
    {
        //Act
        var actual = new LocationViewModel { SelectedRouteIds = new List<string>() };

        //Assert
        actual.RouteData.Should().BeEquivalentTo(new Dictionary<string, string>());
    }

    [Test, AutoData]
    public void Then_The_Route_Data_Dictionary_Is_Built_From_The_Selected_Routes(List<string> routes)
    {
        //Act
        var actual = new LocationViewModel { SelectedRouteIds = routes };

        //Assert
        actual.RouteData.Count.Should().Be(routes.Count);
        actual.RouteData.Values.Should().Contain(routes);
        actual.RouteData.Keys.Should().Contain("routeIds[0]");
        actual.RouteData.Keys.Should().Contain($"routeIds[{routes.Count - 1}]");
    }
}