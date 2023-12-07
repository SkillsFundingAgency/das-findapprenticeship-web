using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenGettingIndex
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_View_Returned(
        GetSearchApprenticeshipsIndexResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        result.LocationSearched = false;
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Index() as ViewResult;

        Assert.IsNotNull(actual);
        actual!.Model.Should().BeEquivalentTo((SearchApprenticeshipsViewModel)result);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Search_View_Returned_When_Searched(
        GetSearchApprenticeshipsIndexResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        result.LocationSearched = false;
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Index(search:1) as RedirectToRouteResult;

        Assert.IsNotNull(actual);
        actual!.RouteName.Should().Be(RouteNames.SearchResults);
    }

    [Test, MoqAutoData]
    public async Task ModelStateIsInvalid_ModelIsReturned(
        string whatSearchTerm,
        string whereSearchTerm,
        GetSearchApprenticeshipsIndexResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        SearchApprenticeshipsViewModel viewModel,
        [Greedy] SearchApprenticeshipsController controller)
    {
        queryResult.LocationSearched = true;
        queryResult.Location = null;
        mediator.Setup(x => x.Send(It.Is<GetSearchApprenticeshipsIndexQuery>(c => c.LocationSearchTerm.Equals(whereSearchTerm)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);
        controller.ModelState.AddModelError("test", "message");

        var result = await controller.Index(whereSearchTerm,whatSearchTerm) as ViewResult;

        result!.Model.Should().BeOfType<SearchApprenticeshipsViewModel>();
    }

    [Test, MoqAutoData]
    public async Task And_ThereIsNoValidationError_SearchResultsReturned(
        string whatSearchTerm,
        string whereSearchTerm,
        GetSearchApprenticeshipsIndexResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        queryResult.LocationSearched = true;
        mediator.Setup(x => x.Send(It.Is<GetSearchApprenticeshipsIndexQuery>(c => c.LocationSearchTerm.Equals(whereSearchTerm)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.Index(whereSearchTerm,whatSearchTerm) as RedirectToRouteResult;

        result!.RouteName.Should().Be(RouteNames.SearchResults);
        result.RouteValues!["location"].Should().Be(queryResult.Location.LocationName);
    }
}
