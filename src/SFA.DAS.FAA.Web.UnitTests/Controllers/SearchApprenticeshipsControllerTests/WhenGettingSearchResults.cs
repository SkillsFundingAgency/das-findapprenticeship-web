using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Application.Queries.GetSearchResults;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.FAA.Web.Validators;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests;

public class WhenGettingSearchResults
{
    [Test]
    [MoqInlineAutoData(2, true)]
    [MoqInlineAutoData(5, true)]
    [MoqInlineAutoData(10, true)]
    [MoqInlineAutoData(15, true)]
    [MoqInlineAutoData(20, true)]
    [MoqInlineAutoData(30, true)]
    [MoqInlineAutoData(40, true)]
    [MoqInlineAutoData(null, true)]
    [MoqInlineAutoData(178, false)]
    [MoqInlineAutoData(1, false)]
    public async Task Then_The_Mediator_Query_Is_Called_And_Search_Results_View_Returned(
        int? distance,
        bool distanceIsValid,
        string mapId,
        GetSearchResultsResult result,
        List<string>? routeIds,
        List<string>? levelIds,
        string? location,
        string? searchTerm,
        int pageNumber,
        bool disabilityConfident,
        VacancySort sort,
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        string routhPath,
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.PageNumber = pageNumber;
        result.Sort = sort.ToString();
        result.VacancyReference = null;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName!.Equals(RouteNames.SearchResults))))
            .Returns("https://baseUrl");
        
        
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );

        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);

        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };
        routeIds = [result.Routes.First().Id.ToString()];
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageNumber!.Equals(pageNumber)
                && c.PageSize!.Equals(10)
                && c.DisabilityConfident!.Equals(disabilityConfident)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            LevelIds = levelIds,
            DisabilityConfident = disabilityConfident,
            RoutePath = routhPath,
            IncludeCompetitiveSalaryVacancies = true
        }) as ViewResult;

        using (new AssertionScope())
        {
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as SearchResultsViewModel;
            actualModel?.Total.Should().Be(((SearchResultsViewModel) result).Total);
            actualModel?.SelectedRouteIds.Should().Equal(routeIds);
            actualModel?.SelectedRouteCount.Should().Be(routeIds.Count);
            actualModel?.SelectedLevelCount.Should().Be(levelIds.Count);
            actualModel?.Location.Should().BeEquivalentTo(location);
            actualModel?.PageNumber.Should().Be(pageNumber);
            actualModel?.VacancyAdverts.Should().NotBeNullOrEmpty();
            actualModel?.MapData.Should().NotBeNullOrEmpty();
            actualModel?.Sort.Should().Be(sort.ToString());
            actualModel?.SelectedRoutes.Should()
                .BeEquivalentTo(result.Routes.Where(c => c.Id.ToString() == routeIds.First()).Select(x => x.Name)
                    .ToList());
            actualModel?.Routes.FirstOrDefault(x => x.Id.ToString() == routeIds.First())?.Selected.Should().BeTrue();
            actualModel?.Routes.Where(x => x.Id.ToString() != routeIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel?.Levels.FirstOrDefault(x => x.Id.ToString() == levelIds.First())?.Selected.Should().BeTrue();
            actualModel?.Levels.Where(x => x.Id.ToString() != levelIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel.DisabilityConfident.Should().Be(disabilityConfident);
            actualModel.ShowAccountCreatedBanner.Should().Be(showBanner);
            actualModel.PageBackLinkRoutePath.Should().NotBeNull();

            switch (actualModel.Total)
            {
                case 0:
                    actualModel.PageTitle.Should()
                        .Be("No results found");
                    break;
                case > 10:
                    actualModel.PageTitle.Should()
                        .Be(
                            $"{actualModel.Total} results found (page {actualModel.PaginationViewModel.CurrentPage} of {actualModel.PaginationViewModel.TotalPages})");
                    break;
                case 1:
                    actualModel.PageTitle.Should()
                            .Be($"{actualModel.Total} result found");
                    break;
                default:
                    actualModel.PageTitle.Should()
                        .Be($"{actualModel.Total} results found");
                    break;
            }

            if (distanceIsValid) 
            {
                actualModel.Distance.Should().Be(distance); 
            }
            else
            {
                actualModel.Distance.Should().Be(10);
            }

            actualModel.MapId.Should().Be(mapId);
            actualModel.ClearSelectedFiltersLink.Should().Be("https://baseUrl");
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_The_Validator_Fails_View_Is_Returned_And_Search_Does_Not_Happen(
        List<string>? routeIds,
        List<string>? levelIds,
        string mapId,
        string govIdentifier,
        string? location,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        int distance,
        bool disabilityConfident,
        Guid candidateId,
        GetSearchResultsResult queryResult,
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<IUrlHelper> mockUrlHelper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult(new List<ValidationFailure>{new ValidationFailure("WhatSearchTerm","Error")})
            );
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };
        
        var actual = await controller.SearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            LevelIds = levelIds,
            DisabilityConfident = disabilityConfident
        }) as ViewResult;

        actual.Should().NotBeNull();
        
        mediator.Verify(x => x.Send(It.IsAny<GetSearchResultsQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_When_Vacancy_Reference_Has_Value_It_Is_Redirected_To_Vacancy_Details(
        List<string>? routeIds,
        List<string>? levelIds,
        string mapId,
        string? location,
        string? searchTerm,
        int pageNumber,
        int pageSize,
        bool disabilityConfident,
        Guid candidateId,
        GetSearchResultsResult queryResult,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        // Arrange
        int? distance = null;
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
                && c.DisabilityConfident!.Equals(disabilityConfident)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };

        // Act
        var actual = await controller.SearchResults (new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            LevelIds = levelIds, 
            DisabilityConfident = disabilityConfident,
        }) as RedirectToRouteResult;

        // Assert
        Assert.That(actual, Is.Not.Null);
        actual!.RouteName.Should().Be(RouteNames.Vacancies);
        actual.RouteValues["VacancyReference"].Should().Be(queryResult.VacancyReference);
    }

    [Test, MoqAutoData]
    public async Task And_The_Request_Distance_Is_Negative_Value_Then_It_Is_Defaulted_To_Null(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        string mapId,
        List<string>? routeIds,
        Guid candidateId,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService)

    {
        request.Distance = -5;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };

        result.VacancyReference = null;
        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.Distance.Should().BeNull();
    }

    [Test]
    [MoqInlineAutoData(3)]
    [MoqInlineAutoData(7)]
    [MoqInlineAutoData(500)]
    public async Task And_Request_Distance_Is_Invalid_Positive_Value_Then_It_Defaults_To_10(
        int distance,
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        List<string>? routeIds,
        Guid candidateId,
        string mapId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        request.Distance = distance;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };

        result.VacancyReference = null;
        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.Distance.Should().Be(10);
    }

    [Test, MoqAutoData]
    public async Task And_The_Request_Page_Number_Is_Invalid_Then_It_Is_Set_To_One(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        string mapId,
        List<string>? routeIds,
        Guid candidateId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        request.Distance = -5;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
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
        string mapId,
        List<string>? routeIds,
        Guid candidateId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        request.PageNumber = -5;
        result.PageNumber = 1;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };
        result.VacancyReference = null;

        mediator.Setup(x => x.Send(It.IsAny<GetSearchResultsQuery>(), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.PageNumber.Should().Be(result.PageNumber);
    }

    
    [Test, MoqAutoData]
    public async Task Then_Sort_Is_Set_To_Distance_When_Location_And_No_Sort_Is_Set(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        string mapId,
        List<string>? routeIds,
        Guid candidateId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        request.Sort = null;
        result.PageNumber = 1;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };
        result.VacancyReference = null;

        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c=>c.Sort == VacancySort.DistanceAsc.ToString()), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.PageNumber.Should().Be(result.PageNumber);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Sort_Is_Set_To_Distance_When_Location_And_Sort_Is_Set_But_No_Other_Filters(
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        string mapId,
        List<string>? routeIds,
        Guid candidateId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        request.Sort = VacancySort.AgeDesc.ToString();
        request.LevelIds = [];
        request.RouteIds = [];
        request.DisabilityConfident = false;
        request.SearchTerm = string.Empty;
        result.PageNumber = 1;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };
        result.VacancyReference = null;

        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c=>c.Sort == VacancySort.DistanceAsc.ToString()), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);

        actualModel?.PageNumber.Should().Be(result.PageNumber);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_No_Search_Results_Found_View_Returned(
        string? location,
        int? distance,
        string mapId,
        GetSearchResultsResult result,
        List<string>? routeIds,
        List<string>? levelIds,
        string? searchTerm,
        int pageNumber,
        bool disabilityConfident,
        VacancySort sort,
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        result.TotalCompetitiveVacanciesCount = 0;
        result.PageNumber = pageNumber;
        result.Sort = sort.ToString();
        result.VacancyReference = null;
        result.VacancyAdverts = [];
        result.Total = 0;
        distance = 3;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship { GoogleMapsId = mapId });
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };
        routeIds = [result.Routes.First().Id.ToString()];
        result.VacancyReference = null;
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageNumber!.Equals(pageNumber)
                && c.PageSize!.Equals(10)
                && c.DisabilityConfident!.Equals(disabilityConfident)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            LevelIds = levelIds,
            DisabilityConfident = disabilityConfident,
            IncludeCompetitiveSalaryVacancies = false
        }) as ViewResult;

        using (new AssertionScope())
        {
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as SearchResultsViewModel;
            actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);
            actualModel?.SelectedRouteIds.Should().Equal(routeIds);
            actualModel?.SelectedRouteCount.Should().Be(routeIds.Count);
            actualModel?.SelectedLevelCount.Should().Be(levelIds!.Count);
            actualModel?.Location.Should().BeEquivalentTo(location);
            actualModel?.PageNumber.Should().Be(pageNumber);
            actualModel?.VacancyAdverts.Should().BeNullOrEmpty();
            actualModel?.Sort.Should().Be(sort.ToString());
            actualModel?.SelectedRoutes.Should()
                .BeEquivalentTo(result.Routes.Where(c => c.Id.ToString() == routeIds.First()).Select(x => x.Name)
                    .ToList());
            actualModel?.Routes.FirstOrDefault(x => x.Id.ToString() == routeIds.First())?.Selected.Should().BeTrue();
            actualModel?.Routes.Where(x => x.Id.ToString() != routeIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel?.Levels.FirstOrDefault(x => x.Id.ToString() == levelIds!.First())?.Selected.Should().BeTrue();
            actualModel?.Levels.Where(x => x.Id.ToString() != levelIds!.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel?.DisabilityConfident.Should().Be(disabilityConfident);
            actualModel?.NoSearchResultsByUnknownLocation.Should().BeFalse();
            actualModel?.Distance.Should().Be(10);
            actualModel?.PageTitle.Should().Be("No results found");
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_No_Search_Results_Found_With_UnknownLocation_View_Returned(
        string? location,
        int? distance,
        string mapId,
        GetSearchResultsResult result,
        List<string>? routeIds,
        List<string>? levelIds,
        string? searchTerm,
        int pageNumber,
        bool disabilityConfident,
        VacancySort sort,
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.PageNumber = pageNumber;
        result.Sort = sort.ToString();
        result.VacancyReference = null;
        result.VacancyAdverts = [];
        result.Location = null;
        result.Total = 0;
        distance = 3;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship { GoogleMapsId = mapId });
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );
        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))

                }
            }
        };
        routeIds = [result.Routes.First().Id.ToString()];
        result.VacancyReference = null;
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageNumber!.Equals(pageNumber)
                && c.PageSize!.Equals(10)
                && c.DisabilityConfident!.Equals(disabilityConfident)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            LevelIds = levelIds,
            DisabilityConfident = disabilityConfident
        }) as ViewResult;

        using (new AssertionScope())
        {
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as SearchResultsViewModel;
            actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);
            actualModel?.SelectedRouteIds.Should().Equal(routeIds);
            actualModel?.SelectedRouteCount.Should().Be(routeIds.Count);
            actualModel?.SelectedLevelCount.Should().Be(levelIds!.Count);
            actualModel?.Location.Should().BeEquivalentTo(location);
            actualModel?.PageNumber.Should().Be(pageNumber);
            actualModel?.VacancyAdverts.Should().BeNullOrEmpty();
            actualModel?.Sort.Should().Be(sort.ToString());
            actualModel?.SelectedRoutes.Should()
                .BeEquivalentTo(result.Routes.Where(c => c.Id.ToString() == routeIds.First()).Select(x => x.Name)
                    .ToList());
            actualModel?.Routes.FirstOrDefault(x => x.Id.ToString() == routeIds.First())?.Selected.Should().BeTrue();
            actualModel?.Routes.Where(x => x.Id.ToString() != routeIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel?.Levels.FirstOrDefault(x => x.Id.ToString() == levelIds!.First())?.Selected.Should().BeTrue();
            actualModel?.Levels.Where(x => x.Id.ToString() != levelIds!.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel?.DisabilityConfident.Should().Be(disabilityConfident);
            actualModel?.NoSearchResultsByUnknownLocation.Should().BeTrue();
            actualModel?.Distance.Should().Be(10);
            actualModel?.PageTitle.Should().Be("No results found");
        }
    }

    [Test]
    [MoqInlineAutoData(VacancySort.SalaryDesc, true, "CompetitiveSalary")]
    [MoqInlineAutoData(VacancySort.SalaryAsc, true, null)]
    [MoqInlineAutoData(VacancySort.AgeAsc, false, null)]
    public async Task Then_Sort_Is_Salary_Type_The_Mediator_Query_Is_Called_And_Search_Results_View_Returned(
        VacancySort sort,
        bool expectedResult,
        string skipWageType,
        int? distance,
        bool distanceIsValid,
        string mapId,
        GetSearchResultsResult result,
        List<string>? routeIds,
        List<string>? levelIds,
        string? location,
        string? searchTerm,
        int pageNumber,
        bool disabilityConfident,
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        string routhPath,
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService)
    {
        result.SkipWageType = skipWageType;
        result.PageNumber = pageNumber;
        result.Sort = sort.ToString();
        result.VacancyReference = null;
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName!.Equals(RouteNames.SearchResults))))
            .Returns("https://baseUrl");


        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship { GoogleMapsId = mapId });
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );

        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);

        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                    }))
                }
            }
        };
        routeIds = [result.Routes.First().Id.ToString()];
        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c =>
                c.SearchTerm!.Equals(searchTerm)
                && c.Location!.Equals(location)
                && c.SelectedRouteIds!.Equals(routeIds)
                && c.PageNumber!.Equals(pageNumber)
                && c.PageSize!.Equals(10)
                && c.DisabilityConfident!.Equals(disabilityConfident)
            ), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.SearchResults(new GetSearchResultsRequest
        {
            Location = location,
            Distance = distance,
            RouteIds = routeIds,
            SearchTerm = searchTerm,
            PageNumber = pageNumber,
            LevelIds = levelIds,
            DisabilityConfident = disabilityConfident,
            RoutePath = routhPath,
            IncludeCompetitiveSalaryVacancies = true,
            Sort = sort.ToString(),
        }) as ViewResult;

        using (new AssertionScope())
        {
            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as SearchResultsViewModel;
            actualModel?.Total.Should().Be(((SearchResultsViewModel)result).Total);
            actualModel?.SelectedRouteIds.Should().Equal(routeIds);
            actualModel?.SelectedRouteCount.Should().Be(routeIds.Count);
            actualModel?.SelectedLevelCount.Should().Be(levelIds.Count);
            actualModel?.Location.Should().BeEquivalentTo(location);
            actualModel?.PageNumber.Should().Be(pageNumber);
            actualModel?.VacancyAdverts.Should().NotBeNullOrEmpty();
            actualModel?.MapData.Should().NotBeNullOrEmpty();
            actualModel?.Sort.Should().Be(sort.ToString());
            actualModel?.SelectedRoutes.Should()
                .BeEquivalentTo(result.Routes.Where(c => c.Id.ToString() == routeIds.First()).Select(x => x.Name)
                    .ToList());
            actualModel?.Routes.FirstOrDefault(x => x.Id.ToString() == routeIds.First())?.Selected.Should().BeTrue();
            actualModel?.Routes.Where(x => x.Id.ToString() != routeIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel?.Levels.FirstOrDefault(x => x.Id.ToString() == levelIds.First())?.Selected.Should().BeTrue();
            actualModel?.Levels.Where(x => x.Id.ToString() != levelIds.First()).Select(x => x.Selected).ToList()
                .TrueForAll(x => x).Should().BeFalse();
            actualModel.DisabilityConfident.Should().Be(disabilityConfident);
            actualModel.ShowAccountCreatedBanner.Should().Be(showBanner);
            actualModel.PageBackLinkRoutePath.Should().NotBeNull();

            switch (actualModel.Total)
            {
                case 0:
                    actualModel.PageTitle.Should()
                        .Be("No results found");
                    break;
                case > 10:
                    actualModel.PageTitle.Should()
                        .Be(
                            $"{actualModel.Total} results found (page {actualModel.PaginationViewModel.CurrentPage} of {actualModel.PaginationViewModel.TotalPages})");
                    break;
                case 1:
                    actualModel.PageTitle.Should()
                            .Be($"{actualModel.Total} result found");
                    break;
                default:
                    actualModel.PageTitle.Should()
                        .Be($"{actualModel.Total} results found");
                    break;
            }
            actualModel.MapId.Should().Be(mapId);
            actualModel.ClearSelectedFiltersLink.Should().Be("https://baseUrl");
            if (expectedResult)
            {
                actualModel.ShowCompetitiveSalaryBanner.Should().BeTrue();
            }
            else
            {
                actualModel.ShowCompetitiveSalaryBanner.Should().BeFalse();
            }

            actualModel.CompetitiveSalaryBannerLinkText.Should().Be(!string.IsNullOrEmpty(skipWageType)
                ? "Show vacancies without a wage"
                : "Hide vacancies without a wage");
        }
    }

    [Test, MoqAutoData]
    public async Task Then_Sort_Is_Set_To_Distance_When_Location_Exist(
        string location,
        GetSearchResultsRequest request,
        GetSearchResultsResult result,
        string mapId,
        List<string>? routeIds,
        Guid candidateId,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IDateTimeService> dateTimeService,
        [Frozen] Mock<GetSearchResultsRequestValidator> validator)
    {
        request.Location = location;
        request.Sort = null;
        request.LevelIds = [];
        request.RouteIds = [];
        request.DisabilityConfident = false;
        result.PageNumber = 1;
        result.Sort = VacancySort.DistanceAsc.ToString();
        var mediator = new Mock<IMediator>();
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("https://baseUrl");
        var faaConfig = new Mock<IOptions<Domain.Configuration.FindAnApprenticeship>>();
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship { GoogleMapsId = mapId });
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<GetSearchResultsRequest>>(), CancellationToken.None))
            .ReturnsAsync(
                new ValidationResult()
            );

        var controller = new SearchApprenticeshipsController(
            mediator.Object,
            dateTimeService.Object,
            faaConfig.Object,
            cacheStorageService.Object,
            Mock.Of<SearchModelValidator>(),
            validator.Object,
            Mock.Of<IDataProtectorService>(),
            Mock.Of<ILogger<SearchApprenticeshipsController>>())
        {
            Url = mockUrlHelper.Object,
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString())
                    }))

                }
            }
        };
        result.VacancyReference = null;

        mediator.Setup(x => x.Send(It.Is<GetSearchResultsQuery>(c => c.Sort == VacancySort.DistanceAsc.ToString()), CancellationToken.None)).ReturnsAsync(result);

        var actual = await controller.SearchResults(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as SearchResultsViewModel;
        actualModel?.Sort.Should().Be(VacancySort.DistanceAsc.ToString());
    }
}
