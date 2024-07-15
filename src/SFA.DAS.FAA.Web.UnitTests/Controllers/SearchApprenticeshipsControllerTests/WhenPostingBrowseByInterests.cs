using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.BrowseByInterests;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenPostingBrowseByInterests
{
    [Test, MoqAutoData]
    public async Task AndModelStateIsInvalid_ModelIsReturned(
        GetBrowseByInterestsResult response,
        BrowseByInterestViewModel model,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        controller.ModelState.AddModelError("test", "message");

        mediator.Setup(x => x.Send(It.IsAny<GetBrowseByInterestsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        var result = await controller.BrowseByInterests(model) as ViewResult;

        result!.Model.Should().BeOfType<BrowseByInterestViewModel>();
    }

    [Test, MoqAutoData]
    public async Task And_ThereIsNoValidationError_LocationsPageIsReturned(
            BrowseByInterestViewModel model,
            [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        var result = await controller.BrowseByInterests(model) as ActionResult;

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Location);
    }
}
