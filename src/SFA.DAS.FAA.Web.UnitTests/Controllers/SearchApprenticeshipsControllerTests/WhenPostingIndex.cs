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

public class WhenPostingIndex
{
    [Test, MoqAutoData]
    public async Task ModelStateIsInvalid_ModelIsReturned(
        SearchApprenticeshipsViewModel viewModel,
        [Greedy] SearchApprenticeshipsController controller)
    {
        controller.ModelState.AddModelError("test", "message");

        var result = await controller.Index(viewModel) as ViewResult;

        result!.Model.Should().BeOfType<SearchApprenticeshipsViewModel>();
    }

    [Test, MoqAutoData]
    public async Task And_ThereIsNoValidationError_SearchResultsReturned(
        SearchApprenticeshipsViewModel viewModel,
        GetIndexLocationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetIndexLocationQuery>(c => c.LocationSearchTerm!.Equals(viewModel.WhereSearchTerm)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await controller.Index(viewModel) as RedirectToRouteResult;

        result!.RouteName.Should().Be(RouteNames.SearchResults);
        result.RouteValues!["location"].Should().Be(viewModel.WhereSearchTerm);
    }
}