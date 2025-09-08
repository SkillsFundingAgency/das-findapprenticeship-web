using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Net;
using Microsoft.Extensions.Options;
using SFA.DAS.FAA.Web.Models.Applications;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FAA.Web.Controllers;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies;

public class WhenGettingVacancyDetails
{
    [Test]
    [MoqInlineAutoData("VAC1000012484","VAC1000012484")]
    [MoqInlineAutoData("1000012484","1000012484")]
    public async Task Then_The_Mediator_Query_Is_Called_And_VacancyDetails_View_Returned(
        string queryVal,
        string vacancyReference,
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        string mapId,
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<IUrlHelper> mockUrlHelper,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] VacanciesController controller)
    {
        // arrange
        request.VacancyReference = vacancyReference;
        mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c=>c.VacancyReference == queryVal), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});

        controller.Url = mockUrlHelper.Object;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                }))

            }
        };

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Vacancy(validator.Object, request, NavigationSource.None, ApplicationsTab.Started) as ViewResult;

        // assert
        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as VacancyDetailsViewModel;

        var expected = VacancyDetailsViewModel.MapToViewModel(dateTimeService, result, mapId);

        actualModel.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.BackLinkUrl)
            .Excluding(x => x.ClosingDate)
            .Excluding(x => x.CourseLevelMapper)
            .Excluding(x => x.FoundationRequirementsUrl)
            .Excluding(x => x.ShowAccountCreatedBanner));
        actualModel!.ShowAccountCreatedBanner.Should().Be(showBanner);
        actualModel.GoogleMapsId.Should().Be(mapId);
        actualModel.Latitude.Should().Be(result.Vacancy!.Location.Lat);
        actualModel.Longitude.Should().Be(result.Vacancy!.Location.Lon);
        actualModel.FoundationRequirementsUrl.Should().Be("#");
    }

    [Test]
    [MoqInlineAutoData("1234567890")]
    [MoqInlineAutoData("VAC12345678")]
    [MoqInlineAutoData("ABC12345678")]
    [MoqInlineAutoData("")]
    public async Task Then_The_Mediator_Query_Is_Called_With_BadRequest_NotFound_Returned(
        string vacancyReference,
        IDateTimeService dateTimeService,
        ValidationResult result,
        Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] VacanciesController controller)
    {
        // arrange
        var request = new GetVacancyDetailsRequest
        {
            VacancyReference = vacancyReference
        };

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        // act
        var actual = await controller.Vacancy(validator.Object, request) as NotFoundResult;

        // assert
        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }


    [Test, MoqAutoData]
    public async Task Then_If_Query_Returns_Null_Then_Not_Found_Returned(
        Guid candidateId,
        Guid govIdentifier,
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] VacanciesController controller)
    {
        // arrange
        result.Vacancy = null;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                }))

            }
        };
        mediator.Setup(x => x.Send(It.IsAny<GetApprenticeshipVacancyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        // act
        var actual = await controller.Vacancy(validator.Object, request) as NotFoundResult;

        // assert
        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }

    [Test]
    [MoqInlineAutoData(NavigationSource.None, null, "https://baseUrl/apprenticeshipsearch", "VAC1000012484","VAC1000012484")]
    [MoqInlineAutoData(NavigationSource.None, null, "", "VAC1000012484", "VAC1000012484")]
    [MoqInlineAutoData(NavigationSource.Applications, null, "https://baseUrl/apprenticeshipsearch", "VAC1000012484", "VAC1000012484")]
    [MoqInlineAutoData(NavigationSource.Applications,"some url", "some url", "1000012484","1000012484")]
    public async Task Then_The_Mediator_Query_Is_Called_And_VacancyDetails_View_Returned_With_Expected_BackLink(
        NavigationSource source,
        string previousPageUrl,
        string expectedUrl,
        string queryVal,
        string vacancyReference,
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        string mapId,
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] VacanciesController controller)
    {
        // arrange
        var mockUrlHelper = new Mock<IUrlHelper>();
        mockUrlHelper
            .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(expectedUrl);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(ctx => ctx.Request.Headers.Referer).Returns(new StringValues(previousPageUrl));

        request.VacancyReference = vacancyReference;
        mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c => c.VacancyReference == queryVal),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship {GoogleMapsId = mapId});

        controller.Url = mockUrlHelper.Object;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, govIdentifier.ToString())
                }))
            },
        };

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult { });

        // act
        var actual = await controller.Vacancy(validator.Object, request, source, ApplicationsTab.Started) as ViewResult;

        // assert
        actual.Should().NotBeNull();
        var actualModel = actual!.Model as VacancyDetailsViewModel;

        actualModel.Should().NotBeNull();
        actualModel?.BackLinkUrl.Should().Be(expectedUrl);
    }

    [Test]
    public void Vacancy_Route_Order_Should_Be_Correct()
    {
        // Arrange
        var methodInfo = typeof(VacanciesController).GetMethod("Vacancy");
        var routeAttributes = methodInfo?.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>().ToList();

        // Act
        var routeOrder1 = routeAttributes?.FirstOrDefault(r => r.Template == "apprenticeship/{vacancyReference}")?.Order;
        var routeOrder2 = routeAttributes?.FirstOrDefault(r => r.Template == "apprenticeship/reference/{vacancyReference}")?.Order;
        var routeOrder3 = routeAttributes?.FirstOrDefault(r => r.Template == "apprenticeship/nhs/{vacancyReference}")?.Order;

        // Assert
        routeOrder1.Should().Be(1);
        routeOrder2.Should().Be(2);
        routeOrder3.Should().Be(3);
    }
}