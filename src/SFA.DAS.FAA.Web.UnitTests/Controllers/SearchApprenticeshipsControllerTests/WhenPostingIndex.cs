using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
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
        [Greedy] SearchApprenticeshipsController controller)
    {
        viewModel.WhereSearchTerm = "Manchester";
        var result = await controller.Index(viewModel) as ActionResult;

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.SearchResults);
    }
}