using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.FAA.Web.Models.BrowseByInterestViewModel;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingBrowseByInterestViewModel
{
    [Test]
    [TestCase("route1")]
    [TestCase("route1, route2")]
    public void Then_The_Result_Is_Converted_In_The_View_Model(string routes)
    {
        var routeList = routes.Split(',').Select(r => r.Trim()).ToList();

        var source = new GetBrowseByInterestsResult()
        {
            Routes = routeList.Select(route => new RouteResponse { Route = route }).ToList()
        };

        var actual = (BrowseByInterestViewModel)source;

        Assert.AreEqual(routeList.Count, actual.Routes.Count);

        for (int i = 0; i < routeList.Count; i++)
        {
            Assert.AreEqual(routeList[i], actual.Routes[i].Route);
        }
    }

    [Test]
    [TestCase("1")]
    public void Then_The_Route_Object_Is_Built_Correctly(string routeId)
    {
        routeObject expectedRouteObject = new routeObject
        {
            routeId = "1",
            routeName = "Agriculture, environmental and animal care",
            displayText = "Agriculture, environmental and animal care",
            hintText = "Farmer, horticulture, vet and similar"
        };

        var viewModel = new BrowseByInterestViewModel();
        viewModel.Routes = new List<RouteViewModel>
        {
            new RouteViewModel
            {
                Id = 1,
                Route = "Agriculture, environmental and animal care"
            }
        };
        
        viewModel.allocateRouteGroup();

        routeObject actualRouteObject = viewModel.agriculutreEnvironmentalAndAnimalCareDictionary[routeId];

 
        Assert.AreEqual(expectedRouteObject.routeId, actualRouteObject.routeId);
        Assert.AreEqual(expectedRouteObject.routeName, actualRouteObject.routeName);
        Assert.AreEqual(expectedRouteObject.displayText, actualRouteObject.displayText);
        Assert.AreEqual(expectedRouteObject.hintText, actualRouteObject.hintText);
    }
}