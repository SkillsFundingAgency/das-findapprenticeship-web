using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingLocations
{
    [Test, MoqAutoData]
    public void Then_TheViewModelRoutesAreSet_AndViewReturned(
        List<string>? routeIds,
        [Greedy] SearchApprenticeshipsController controller)
    {
        var actual = controller.Location(routeIds) as ViewResult;

        using (new AssertionScope())
        {
            actual!.Model.Should().BeOfType<LocationViewModel>();
            actual.Model.As<LocationViewModel>().RouteData.Count.Equals(routeIds?.Count);
        }
    }
}
