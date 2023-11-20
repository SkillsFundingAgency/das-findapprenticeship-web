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
            Assert.AreEqual(routeList[i], actual.Routes[i].Name);
        }
    }

    [Test]
    [TestCase(1, "Agriculture, environmental and animal care")]
    public void And_There_Are_PreviousRouteIds_Then_The_Route_Object_Is_Built_Correctly(int routeId, string route)
    {
        RouteObject expectedRouteObject1 = new RouteObject
        {
            RouteId = "1",
            RouteName = "Agriculture, environmental and animal care",
            DisplayText = "Agriculture, environmental and animal care",
            HintText = "Farmer, horticulture, vet and similar",
            PreviouslySelected = true
        };
        var previouslySelectedRouteIds = new List<string> { "1", "5" };

        var viewModel = new BrowseByInterestViewModel();
        viewModel.Routes = new List<RouteViewModel>
        {
            new RouteViewModel
            {
                Id = routeId,
                Name = route
            }
        };
        
        viewModel.AllocateRouteGroup(previouslySelectedRouteIds);

        RouteObject actualRouteObject = viewModel.AgricultureEnvironmentalAndAnimalCareDictionary[routeId.ToString()];


        Assert.Multiple(() =>
        {
            Assert.AreEqual(expectedRouteObject1.RouteId, actualRouteObject.RouteId);
            Assert.AreEqual(expectedRouteObject1.RouteName, actualRouteObject.RouteName);
            Assert.AreEqual(expectedRouteObject1.DisplayText, actualRouteObject.DisplayText);
            Assert.AreEqual(expectedRouteObject1.HintText, actualRouteObject.HintText);
            Assert.AreEqual(expectedRouteObject1.PreviouslySelected, actualRouteObject.PreviouslySelected);
        });

    }

    [Test, TestCase(15, "Transport and logistics")]
    public void And_There_Are_No_PreviousRouteIds_Then_The_Route_Object_Is_Built_Correctly(int routeId, string route)
    {
        RouteObject expectedRouteObject = new RouteObject
        {
            RouteId = "15",
            RouteName = "Transport and logistics",
            DisplayText = "Transport and logistics",
            HintText = "Aviation ground handler, train driver, fleet manager and similar",
            PreviouslySelected = false
        };
        var previouslySelectedRouteIds = new List<string> { "1", "2", "12" };

        var viewModel = new BrowseByInterestViewModel();
        viewModel.Routes = new List<RouteViewModel>
        {
            new RouteViewModel
            {
                Id = routeId,
                Name = route
            }
        };

        viewModel.AllocateRouteGroup(previouslySelectedRouteIds);
        RouteObject actualRouteObject = viewModel.TransportAndLogisticsDictionary[routeId.ToString()];


        Assert.Multiple(() =>
        {
            Assert.AreEqual(expectedRouteObject.RouteId, actualRouteObject.RouteId);
            Assert.AreEqual(expectedRouteObject.RouteName, actualRouteObject.RouteName);
            Assert.AreEqual(expectedRouteObject.DisplayText, actualRouteObject.DisplayText);
            Assert.AreEqual(expectedRouteObject.HintText, actualRouteObject.HintText);
            Assert.AreEqual(expectedRouteObject.PreviouslySelected, actualRouteObject.PreviouslySelected);
        });
    }

    [Test]
    public void Then_The_Route_Object_Is_Added_To_A_Dictionary()
    {
        // Arrange
        var viewModel = new BrowseByInterestViewModel
        {
            Routes = new List<RouteViewModel>
            {
                new RouteViewModel { Id = 2, Name = "Business Administration" },
                new RouteViewModel { Id = 14, Name = "Sales and Marketing" },
                new RouteViewModel { Id = 12, Name = "Legal, Finance, Accounting" }
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