using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Search_Results_View_Returned(
        GetSearchResultsResult result,
        [Frozen] Mock<IMediator> mediator,
        List<string>? routeIds,
        string? location,
        int distance,
        [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(routeIds, location, distance) as ViewResult;

        Assert.IsNotNull(actual);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel.Total.Should().Be(((SearchResultsViewModel)result).Total);
        actualModel.SelectedRouteIds.Should().Equal(routeIds);
        actualModel.Location.Should().BeEquivalentTo(location);
        actualModel.Distance.Should().Be(distance);
        actualModel.vacancies.Should().NotBeNullOrEmpty();

    }

}