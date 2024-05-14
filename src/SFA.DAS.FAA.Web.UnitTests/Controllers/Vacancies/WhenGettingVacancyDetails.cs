using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
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

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies;

public class WhenGettingVacancyDetails
{
    [Test, MoqAutoData]
    public async Task Then_The_Mediator_Query_Is_Called_And_VacancyDetails_View_Returned(
        Guid candidateId,
        Guid govIdentifier,
        bool showBanner,
        GetApprenticeshipVacancyQueryResult result,
        GetVacancyDetailsRequest request,
        IDateTimeService dateTimeService,
        [Frozen] Mock<IMediator> mediator,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] Web.Controllers.VacanciesController controller)
    {
        mediator.Setup(x => x.Send(It.IsAny<GetApprenticeshipVacancyQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(result);
        cacheStorageService
            .Setup(x => x.Get<bool>($"{govIdentifier}-{CacheKeys.AccountCreated}"))
            .ReturnsAsync(showBanner);

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

        var actual = await controller.Vacancy(request) as ViewResult;

        Assert.That(actual, Is.Not.Null);
        var actualModel = actual!.Model as VacancyDetailsViewModel;

        var expected = new VacancyDetailsViewModel().MapToViewModel(dateTimeService, result);

        actualModel.Should().BeEquivalentTo(expected, options => options
            .Excluding(x => x.ClosingDate)
            .Excluding(x => x.CourseLevelMapper)
            .Excluding(x => x.ShowAccountCreatedBanner));
        actualModel!.ShowAccountCreatedBanner.Should().Be(showBanner);
    }
}