using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_TheViewModelRoutesAndLocationAreSet_AndViewReturned(
        List<string>? routeIds,
        string? location,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        // Act
        var result = await controller.SearchResults(routeIds, location);

        // Assert
        result.Should().BeAssignableTo<ViewResult>();
        var viewResult = result as ViewResult;
        viewResult.Should().NotBeNull();
        viewResult.Model.Should().BeAssignableTo<SearchResultsViewModel>();

        var viewModel = viewResult.Model as SearchResultsViewModel;
        viewModel.SelectedRouteIds.Should().Equal(routeIds);
        viewModel.location.Should().BeEquivalentTo(location);
    }
}