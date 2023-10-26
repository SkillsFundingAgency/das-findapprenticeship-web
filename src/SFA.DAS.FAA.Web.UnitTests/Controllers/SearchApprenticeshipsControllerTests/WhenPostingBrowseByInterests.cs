using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenPostingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task And_ThereIsNoValidationError_LocationsPageIsReturned(
            BrowseByInterestRequestViewModel model,
            [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        model.ErrorDictionary.Clear();

        var result = await controller.BrowseByInterests(model) as ActionResult;

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Location);
    }
}
