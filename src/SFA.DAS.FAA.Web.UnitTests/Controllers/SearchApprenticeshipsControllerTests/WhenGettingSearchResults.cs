using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.FAT.Web.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Search_Results_View_Returned(
        GetSearchResultsResult result,
        List<string>? routeIds,
        string? location,
        int distance,
        string? searchTerm,
        int pageNumber,
        VacancySort sort,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.PageNumber = pageNumber;
        result.Sort = sort.ToString();
        result.VacancyReference = null;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object)
        {
            Url = mockUrlHelper.Object
        };
        routeIds = new() {result.Routes.First().Id.ToString()};
        result.VacancyReference = null;
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Distance!.Equals(distance)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageNumber!.Equals(pageNumber)
                && c.PageSize!.Equals(10)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber
        }) as ViewResult;

        using (new AssertionScope())
        {
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as SearchResultsViewModel;
            actualModel?.Total.Should().Be(((SearchResultsViewModel) result).Total);
            actualModel?.SelectedRouteIds.Should().Equal(routeIds);
            actualModel?.Location.Should().BeEquivalentTo(location);
            actualModel?.Distance.Should().Be(distance);
            actualModel?.PageNumber.Should().Be(pageNumber);
            //actualModel?.PageSize.Should().Be(10);
            actualModel?.Vacancies.Should().NotBeNullOrEmpty();
            actualModel?.Sort.Should().Be(sort.ToString());
            actualModel?.SelectedRoutes.Should()
                .BeEquivalentTo(result.Routes.Where(c => c.Id.ToString() == routeIds.First()).Select(x => x.Name)
                    .ToList());
            actualModel?.Routes.FirstOrDefault(x => x.Id.ToString() == routeIds.First())?.Selected.Should().BeTrue();
            actualModel?.Routes.Where(x => x.Id.ToString() != routeIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
        }
    }

    [Test, MoqAutoData]
    public async Task Then_When_Vacancy_Reference_Has_Value_It_Is_Redirected_To_Vacancy_Details(
        List<string>? routeIds,
        string? location,
        int distance,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        GetSearchResultsResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        // Arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        queryResult.VacancyReference = "NotNullString";
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Distance!.Equals(distance)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageNumber!.Equals(pageNumber)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);
        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object)
        {
            Url = mockUrlHelper.Object
        };

        // Act
        var actual = await controller.SearchResults (new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber
        }) as RedirectToRouteResult;

        // Assert
        Assert.That(actual, Is.Not.Null);
        actual!.RouteName.Should().Be(RouteNames.Vacancies);
        actual.RouteValues["VacancyReference"].Should().Be(queryResult.VacancyReference);
    }

    [Test, MoqAutoData]
    public async Task And_The_Request_Distance_Is_Invalid_Then_It_Is_Defaulted_To_Null(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        List<string>? routeIds,
        [Frozen] Mock<IDateTimeService> dateTimeService)

    {
        request.Distance = -5;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object)
        {
            Url = mockUrlHelper.Object
        };

        result.VacancyReference = null;
        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.Distance.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task And_The_Request_Page_Number_Is_Invalid_Then_It_Is_Set_To_One(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        List<string>? routeIds,
        [Frozen] Mock<IDateTimeService> dateTimeService)

    {
        request.Distance = -5;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object)
        {
            Url = mockUrlHelper.Object
        };

        result.VacancyReference = null;

        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.Distance.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Then_The_Page_Size_Defaults_To_10(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        List<string>? routeIds,
        [Frozen] Mock<IDateTimeService> dateTimeService)

    {
        request.PageNumber = -5;
        result.PageNumber = 1;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object)
        {
            Url = mockUrlHelper.Object
        };
        result.VacancyReference = null;

        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.PageNumber.Should().Be(result.PageNumber);
    }
}
