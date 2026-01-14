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
    [TestCase(2, "e.g. administrator, project manager, human resources")]
    [TestCase(12, "e.g. financial advisor, payroll assistant, solicitor")]
    [TestCase(13, "e.g. security, firefighter, coastguard")]
    [TestCase(14, "e.g. sales manager, digital marketer")]
    [TestCase(15, "e.g. aviation ground handler, train driver, fleet manager")]
    public void Then_The_Business_Category_Is_Built_Correctly(int routeId, string expectedHintText)
    {
        // arrange
        var expectedRouteObject = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = routeId.ToString(),
            RouteName = "Route name text",
            DisplayText = "Route name text",
            HintText = expectedHintText,
            PreviouslySelected = true
        };

        var viewModel = new BrowseByInterestViewModel
        {
            Routes = [ new RouteViewModel { Id = routeId, Name = "Route name text" } ]
        };
        
        // act
        viewModel.AllocateRouteGroup([routeId.ToString()]);

        // assert
        viewModel.BusinessFinanceAndPublicServicesDictionary[routeId.ToString()].Should().BeEquivalentTo(expectedRouteObject);
    }
    
    [Test]
    [TestCase(4, "e.g. chef, baker, hospitality staff")]
    [TestCase(6, "e.g. graphic designer, photographer, animator")]
    [TestCase(10, "e.g. hairdresser, barber, holistic therapist")]
    public void Then_The_Creative_Category_Is_Built_Correctly(int routeId, string expectedHintText)
    {
        // arrange
        var expectedRouteObject = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = routeId.ToString(),
            RouteName = "Route name text",
            DisplayText = "Route name text",
            HintText = expectedHintText,
            PreviouslySelected = false
        };

        var viewModel = new BrowseByInterestViewModel
        {
            Routes = [ new RouteViewModel { Id = routeId, Name = "Route name text" } ]
        };
        
        // act
        viewModel.AllocateRouteGroup([]);

        // assert
        viewModel.CreativeAndServiceIndustriesDictionary[routeId.ToString()].Should().BeEquivalentTo(expectedRouteObject);
    }
    
    [Test]
    [TestCase(1, "e.g. farmer, gardener, vet")]
    public void Then_The_Environment_Category_Is_Built_Correctly(int routeId, string expectedHintText)
    {
        // arrange
        var expectedRouteObject = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = routeId.ToString(),
            RouteName = "Route name text",
            DisplayText = "Route name text",
            HintText = expectedHintText,
            PreviouslySelected = false
        };

        var viewModel = new BrowseByInterestViewModel
        {
            Routes = [ new RouteViewModel { Id = routeId, Name = "Route name text" } ]
        };
        
        // act
        viewModel.AllocateRouteGroup([]);

        // assert
        viewModel.EnvironmentAndAgricultureDictionary[routeId.ToString()].Should().BeEquivalentTo(expectedRouteObject);
    }
    
    [Test]
    [TestCase(3, "e.g. social worker, play therapist, adult carer")]
    [TestCase(8, "e.g. teaching assistant, early years educator")]
    [TestCase(11, "e.g. ambulance worker, sports coach, clinical scientist")]
    public void Then_The_People_Category_Is_Built_Correctly(int routeId, string expectedHintText)
    {
        // arrange
        var expectedRouteObject = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = routeId.ToString(),
            RouteName = "Route name text",
            DisplayText = "Route name text",
            HintText = expectedHintText,
            PreviouslySelected = false
        };

        var viewModel = new BrowseByInterestViewModel
        {
            Routes = [ new RouteViewModel { Id = routeId, Name = "Route name text" } ]
        };
        
        // act
        viewModel.AllocateRouteGroup([]);

        // assert
        viewModel.PeopleAndCareDictionary[routeId.ToString()].Should().BeEquivalentTo(expectedRouteObject);
    }
    
    
    [Test]
    [TestCase(5, "Construction and building", "e.g. bricklayer, carpenter, architect")]
    [TestCase(7, "Route name text", "e.g. software engineer, data scientist, cloud engineer")]
    [TestCase(9, "Route name text", "e.g. autocare technician, welder, rail engineer")]
    public void Then_The_Technical_Category_Is_Built_Correctly(int routeId, string expectedDisplayText, string expectedHintText)
    {
        // arrange
        var expectedRouteObject = new BrowseByInterestViewModel.RouteObject
        {
            RouteId = routeId.ToString(),
            RouteName = "Route name text",
            DisplayText = expectedDisplayText,
            HintText = expectedHintText,
            PreviouslySelected = false
        };

        var viewModel = new BrowseByInterestViewModel
        {
            Routes = [ new RouteViewModel { Id = routeId, Name = "Route name text" } ]
        };
        
        // act
        viewModel.AllocateRouteGroup([]);

        // assert
        viewModel.TechnicalAndEngineeringDictionary[routeId.ToString()].Should().BeEquivalentTo(expectedRouteObject);
    }
}