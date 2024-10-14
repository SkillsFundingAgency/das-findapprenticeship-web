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
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.FAA.Web.Validators;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;
public class WhenGettingIndex
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_Index_View_Returned(
        GetSearchApprenticeshipsIndexResult result,
        Guid govIdentifier,
        bool showBanner,
        [Frozen] Mock<SearchModelValidator> validator,
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
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<SearchModel>>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        cacheStorageService
        .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
        .ReturnsAsync(showBanner);

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object, validator.Object,Mock.Of<GetSearchResultsRequestValidator>())
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

        var actual = await controller.Index(new SearchModel()) as ViewResult;

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
        [Frozen] Mock<SearchModelValidator> validator,
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
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<SearchModel>>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object, validator.Object,Mock.Of<GetSearchResultsRequestValidator>())
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
        var actual = await controller.Index(new SearchModel(),search:1) as RedirectToRouteResult;

        Assert.That(actual, Is.Not.Null);
        actual!.RouteName.Should().Be(RouteNames.SearchResults);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Validator_Fails_Error_And_View_Returned(
        string whatSearchTerm,
        string whereSearchTerm,
        Guid govIdentifier,
        GetSearchApprenticeshipsIndexResult queryResult,
        SearchApprenticeshipsViewModel viewModel,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<SearchModelValidator> validator,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<ICacheStorageService> cacheStorageService)
    {
        queryResult.LocationSearched = false;
        queryResult.Location = null;
        mediator.Setup(x => x.Send(It.Is<GetSearchApprenticeshipsIndexQuery>(c => c.LocationSearchTerm.Equals(whereSearchTerm)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<SearchModel>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult(new List<ValidationFailure>{new ValidationFailure("WhatSearchTerm","Error")})
                );

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object, validator.Object,Mock.Of<GetSearchResultsRequestValidator>())
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
        var model = new SearchModel
        {
            WhereSearchTerm = whereSearchTerm,
            WhatSearchTerm = whatSearchTerm
        };
        var result = await controller.Index(model,1) as ViewResult;

        result!.Model.Should().BeOfType<SearchApprenticeshipsViewModel>();
    }
    [Test, MoqAutoData]
    public async Task ModelStateIsInvalid_ModelIsReturned(
        string whatSearchTerm,
        string whereSearchTerm,
        Guid govIdentifier,
        GetSearchApprenticeshipsIndexResult queryResult,
        SearchApprenticeshipsViewModel viewModel,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<SearchModelValidator> validator,
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
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<SearchModel>>(), CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        var controller = new SearchApprenticeshipsController(mediator.Object, dateTimeService.Object,Mock.Of<IOptions<Domain.Configuration.FindAnApprenticeship>>(), cacheStorageService.Object, validator.Object,Mock.Of<GetSearchResultsRequestValidator>())
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
        var model = new SearchModel
        {
            WhereSearchTerm = whereSearchTerm,
            WhatSearchTerm = whatSearchTerm
        };
        var result = await controller.Index(model) as ViewResult;

        result!.Model.Should().BeOfType<SearchApprenticeshipsViewModel>();
    }

    [Test, MoqAutoData]
    public async Task And_ThereIsNoValidationError_SearchResultsReturned(
        string whereSearchTerm,
        GetSearchApprenticeshipsIndexResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] SearchApprenticeshipsController controller)
    {
        queryResult.LocationSearched = true;
        mediator.Setup(x => x.Send(It.Is<GetSearchApprenticeshipsIndexQuery>(c => c.LocationSearchTerm.Equals(whereSearchTerm)), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);
        var model = new SearchModel
        {
            WhereSearchTerm = whereSearchTerm
        };
        var result = await controller.Index(model) as RedirectToRouteResult;

        result!.RouteName.Should().Be(RouteNames.SearchResults);
        result.RouteValues!["location"].Should().Be(queryResult.Location.LocationName);
        result.RouteValues!["sort"].Should().Be(VacancySort.DistanceAsc);
    }
}
