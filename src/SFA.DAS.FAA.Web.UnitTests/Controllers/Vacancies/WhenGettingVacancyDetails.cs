using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies;

public class WhenGettingVacancyDetails
{
    [Test]
    [MoqInlineAutoData("VAC1000012484","VAC1000012484")]
    [MoqInlineAutoData("VAC1000012484","1000012484")]
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
        [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
        [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
        request.VacancyReference = vacancyReference;
        mediator.Setup(x => x.Send(It.Is<GetApprenticeshipVacancyQuery>(c=>c.VacancyReference == queryVal), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);
        faaConfig.Setup(x => x.Value).Returns(new Domain.Configuration.FindAnApprenticeship{GoogleMapsId = mapId});

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
            .ReturnsAsync(new ValidationResult { });

        var actual = await controller.Vacancy(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as VacancyDetailsViewModel;

        var expected = new VacancyDetailsViewModel().MapToViewModel(dateTimeService, result, mapId);

        actualModel.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.ClosingDate)
            .Excluding(x => x.CourseLevelMapper)
            .Excluding(x => x.ShowAccountCreatedBanner));
        actualModel!.ShowAccountCreatedBanner.Should().Be(showBanner);
        actualModel.GoogleMapsId.Should().Be(mapId);
        actualModel.Latitude.Should().Be(result.Vacancy!.Location.Lat);
        actualModel.Longitude.Should().Be(result.Vacancy!.Location.Lon);
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
        [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
        var request = new GetVacancyDetailsRequest
        {
            VacancyReference = vacancyReference
        };

        validator.Setup(x => x.ValidateAsync(request, It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);

        var actual = await controller.Vacancy(request) as NotFoundResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }


    [Test, MoqAutoData]
    public async Task Then_If_Query_Returns_Null_Then_Not_Found_Returned(
        Guid candidateId,
        Guid govIdentifier,
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
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
            .ReturnsAsync(new ValidationResult { });

        var actual = await controller.Vacancy(request) as NotFoundResult;

        actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    
}