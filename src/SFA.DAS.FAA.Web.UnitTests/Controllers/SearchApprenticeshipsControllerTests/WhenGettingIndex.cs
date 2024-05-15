using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.SearchApprenticeshipsIndex;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Services;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenGettingIndex
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_View_Returned(
        GetSearchApprenticeshipsIndexResult result,
        Guid govIdentifier,
        bool showBanner,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<IMediator> mediator)
    {
        result.LocationSearched = false;
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        cacheStorageService
        .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
        .ReturnsAsync(showBanner);

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };

        var actual = await controller.Index() as ViewResult;

        Assert.That(actual, Is.Not.Null);
        actual!.Model.Should().BeEquivalentTo((SearchApprenticeshipsViewModel)result, options => options.Excluding(prop => prop.ShowAccountCreatedBanner));
        var actualModel = actual!.Model as SearchApprenticeshipsViewModel;
        Assert.That(actualModel, Is.Not.Null);
        actualModel!.ShowAccountCreatedBanner.Should().Be(showBanner);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Search_View_Returned_When_Searched(
        GetSearchApprenticeshipsIndexResult result,
        Guid govIdentifier,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.LocationSearched = false;
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        mediator.Setup(x => x.Send(It.IsAny<GetSearchApprenticeshipsIndexQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };
        var actual = await controller.Index(search:1) as RedirectToRouteResult;

        Assert.That(actual, Is.Not.Null);
        actual!.RouteName.Should().Be(RouteNames.SearchResults);
    }

    [Test, MoqAutoData]
    public async Task ModelStateIsInvalid_ModelIsReturned(
        string whatSearchTerm,
        string whereSearchTerm,
        Guid govIdentifier,
        GetSearchApprenticeshipsIndexResult queryResult,
        SearchApprenticeshipsViewModel viewModel,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        queryResult.LocationSearched = true;
        queryResult.Location = null;
        mediator.Setup(x => x.Send(It.Is<GetSearchApprenticeshipsIndexQuery>(c => c.LocationSearchTerm.Equals(whereSearchTerm)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object)
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };
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
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
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
