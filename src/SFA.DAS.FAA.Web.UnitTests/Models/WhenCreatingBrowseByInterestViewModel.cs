using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Web.Models;
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
            Routes = routeList.Select(route => new RouteResponse { Name = route }).ToList()
        };

        var actual = (BrowseByInterestViewModel)source;

        Assert.AreEqual(routeList.Count, actual.Routes.Count);

        for (int i = 0; i < routeList.Count; i++)
        {
            Assert.AreEqual(routeList[i], actual.Routes[i].Route);
        }
    }

    [Test]
    [TestCase(1, "Agriculture, environmental and animal care")]
    public void Then_The_Route_Object_Is_Built_Correctly(int routeId, string route)
    {
        RouteObject expectedRouteObject = new RouteObject
        {
            RouteId = "1",
            RouteName = "Agriculture, environmental and animal care",
            DisplayText = "Agriculture, environmental and animal care",
            HintText = "Farmer, horticulture, vet and similar"
        };

        var viewModel = new BrowseByInterestViewModel();
        viewModel.Routes = new List<RouteViewModel>
        {
            new RouteViewModel
            {
                Id = routeId,
                Route = route
            }
        };
        
        viewModel.AllocateRouteGroup();

        RouteObject actualRouteObject = viewModel.AgricultureEnvironmentalAndAnimalCareDictionary[routeId.ToString()];

 
        Assert.AreEqual(expectedRouteObject.RouteId, actualRouteObject.RouteId);
        Assert.AreEqual(expectedRouteObject.RouteName, actualRouteObject.RouteName);
        Assert.AreEqual(expectedRouteObject.DisplayText, actualRouteObject.DisplayText);
        Assert.AreEqual(expectedRouteObject.HintText, actualRouteObject.HintText);
    }

    [Test]
    public void Then_The_Route_Object_Is_Added_To_A_Dictionary()
    {
        // Arrange
        var viewModel = new BrowseByInterestViewModel
        {
            Routes = new List<BrowseByInterestViewModel.RouteViewModel>
            {
                new RouteViewModel { Id = 2, Route = "Business Administration" },
                new RouteViewModel { Id = 14, Route = "Sales and Marketing" },
                new RouteViewModel { Id = 12, Route = "Legal, Finance, Accounting" }
            }
        };

        // Act
        viewModel.AllocateRouteGroup();

        // Assert
        Assert.IsTrue(viewModel.BusinessSalesAndLegalDictionary.ContainsKey("2"));
        Assert.IsTrue(viewModel.BusinessSalesAndLegalDictionary.ContainsKey("14"));
        Assert.IsTrue(viewModel.BusinessSalesAndLegalDictionary.ContainsKey("12"));

        // You can also assert the values if needed
        Assert.AreEqual("Business Administration", viewModel.BusinessSalesAndLegalDictionary["2"].RouteName);
        Assert.AreEqual("Sales and Marketing", viewModel.BusinessSalesAndLegalDictionary["14"].RouteName);
        Assert.AreEqual("Legal, Finance, Accounting", viewModel.BusinessSalesAndLegalDictionary["12"].RouteName);
    }

}