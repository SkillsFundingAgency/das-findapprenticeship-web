using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingSearchResults
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Search_Results_View_Returned(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        List<string>? routeIds,
        [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IDateTimeService> dateTimeService)

    {
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object, validator.Object)
        {
            Url = mockUrlHelper.Object
        };

        routeIds = new List<string> { result.Routes.First().Id.ToString() };
        
        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.IsNotNull(actual);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);
        
        actualModel?.Location.Should().BeEquivalentTo(request.Location);
        actualModel?.Distance.Should().Be(request.Distance);
        actualModel?.Vacancies.Should().NotBeNullOrEmpty();
        actualModel?.SelectedRouteIds?.Count.Should().Be(request.RouteIds?.Count);

        //TODO: Will be implemented in the upcoming stories.
        //if (actualModel.SelectedRoutes != null)
        //{
        //    actualModel.SelectedRoutes.Should()
        //        .BeEquivalentTo(result.Routes.Where(c => c.Id.ToString() == routeIds.First()).Select(x => x.Name).ToList());
        //    actualModel.Routes.FirstOrDefault(x => x.Id.ToString() == routeIds.First()).Selected.Should().BeTrue();
        //    actualModel.Routes.Where(x => x.Id.ToString() != routeIds.First()).Select(x => x.Selected).ToList()
        //        .TrueForAll(x => x).Should().BeFalse();
        //}
    }
}