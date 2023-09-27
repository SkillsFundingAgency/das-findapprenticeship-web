using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Domain.BrowseByInterests;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

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
}