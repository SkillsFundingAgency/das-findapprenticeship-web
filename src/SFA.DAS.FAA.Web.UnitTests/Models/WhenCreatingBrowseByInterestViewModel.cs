using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;

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

        var actual = BrowseByInterestViewModel.ToViewModel(source);

        Assert.That(actual.Routes.Count, Is.EqualTo(routeList.Count));
        
        for (var i = 0; i < routeList.Count; i++)
        {
            Assert.That(actual.Routes[i].Name, Is.EqualTo(routeList[i]));
        }
    }

    [Test]
    [TestCase(1, "Agriculture, environmental and animal care")]
    public void And_There_Are_PreviousRouteIds_Then_The_Route_Object_Is_Built_Correctly(int routeId, string route)
    {
        var expectedRouteObject1 = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = "1",
            RouteName = "Agriculture, environmental and animal care",
            DisplayText = "Agriculture, environmental and animal care",
            HintText = "e.g. farmer, gardener, vet",
            PreviouslySelected = true
        };
        var previouslySelectedRouteIds = new List<string> { "1", "5" };

        var viewModel = new BrowseByInterestViewModel();
        viewModel.Routes = new List<RouteViewModel>
        {
            new()
            {
                Id = routeId,
                Name = route
            }
        };
        
        viewModel.AllocateRouteGroup(previouslySelectedRouteIds);

        var actualRouteObject = viewModel.EnvironmentAndAgricultureDictionary[routeId.ToString()];

        actualRouteObject.Should().BeEquivalentTo(expectedRouteObject1);
    }

    [Test, TestCase(15, "Transport and logistics")]
    public void And_There_Are_No_PreviousRouteIds_Then_The_Route_Object_Is_Built_Correctly(int routeId, string route)
    {
        var expectedRouteObject = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = "15",
            RouteName = "Transport and logistics",
            DisplayText = "Transport and logistics",
            HintText = "e.g. aviation ground handler, train driver, fleet manager",
            PreviouslySelected = false
        };
        var previouslySelectedRouteIds = new List<string> { "1", "2", "12" };
    
        var viewModel = new BrowseByInterestViewModel
        {
            Routes = new List<RouteViewModel>
            {
                new()
                {
                    Id = routeId,
                    Name = route
                }
            }
        };
    
        viewModel.AllocateRouteGroup(previouslySelectedRouteIds);
        var actualRouteObject = viewModel.BusinessFinanceAndPublicServicesDictionary[routeId.ToString()];
    
        actualRouteObject.Should().BeEquivalentTo(expectedRouteObject);
    }
    
    [Test]
    public void Then_The_Route_Object_Is_Added_To_A_Dictionary()
    {
        // Arrange
        var viewModel = new BrowseByInterestViewModel
        {
            Routes = new List<RouteViewModel>
            {
                new() { Id = 2, Name = "Business Administration" },
                new() { Id = 14, Name = "Sales and Marketing" },
                new() { Id = 12, Name = "Legal, Finance, Accounting" }
            }
        };
    
        // Act
        viewModel.AllocateRouteGroup();                     
    
        // Assert
        Assert.That(viewModel.BusinessFinanceAndPublicServicesDictionary.ContainsKey("2"), Is.True);
        Assert.That(viewModel.BusinessFinanceAndPublicServicesDictionary.ContainsKey("14"), Is.True);
        Assert.That(viewModel.BusinessFinanceAndPublicServicesDictionary.ContainsKey("12"), Is.True);
    
        // You can also assert the values if needed
        Assert.That(viewModel.BusinessFinanceAndPublicServicesDictionary["2"].RouteName, Is.EqualTo("Business Administration"));
        Assert.That(viewModel.BusinessFinanceAndPublicServicesDictionary["14"].RouteName, Is.EqualTo("Sales and Marketing"));
        Assert.That(viewModel.BusinessFinanceAndPublicServicesDictionary["12"].RouteName, Is.EqualTo("Legal, Finance, Accounting"));
    }
}