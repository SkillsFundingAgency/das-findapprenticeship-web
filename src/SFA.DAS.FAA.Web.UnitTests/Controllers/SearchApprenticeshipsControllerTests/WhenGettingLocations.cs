using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenGettingLocations
{
    [Test, MoqAutoData]
    public void Then_TheViewModelRoutesAreSet_AndViewReturned(
        List<string>? routeIds,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        var actual = controller.Location(routeIds) as ViewResult;

        using (new AssertionScope())
        {
            actual!.Model.Should().BeOfType<LocationViewModel>();
            actual.Model.As<LocationViewModel>().RouteData.Count.Equals(routeIds?.Count);
        }
    }
}
