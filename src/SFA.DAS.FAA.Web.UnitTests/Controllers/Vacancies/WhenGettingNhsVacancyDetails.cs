using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SFA.DAS.FAA.Application.Queries.GetNhsApprenticeshipVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Vacancy;
using SFA.DAS.FAT.Domain.Interfaces;
using System.Security.Claims;
using SFA.DAS.FAA.Application.Queries.GetApprenticeshipVacancy;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Vacancies
{
    [TestFixture]
    public class WhenGettingNhsVacancyDetails
    {
        [Test]
        [MoqInlineAutoData("https://baseUrl/apprenticeshipsearch")]
        [MoqInlineAutoData("https://baseUrl/apprenticeshipsearch?query1=somevalue&query2=someValue2")]
        public async Task Then_The_Mediator_Query_Is_Called_And_VacancyDetails_View_Returned(string redirectUrl,
            string vacancyReference,
            Guid candidateId,
            GetNhsApprenticeshipVacancyQueryResult result,
            GetVacancyDetailsRequest request,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
            [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Greedy] VacanciesController controller)
        {
            request.VacancyReference = vacancyReference;
            mediator.Setup(x => x.Send(It.Is<GetNhsApprenticeshipVacancyQuery>(c => c.VacancyReference == vacancyReference), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(redirectUrl);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(ctx => ctx.Request.Headers.Referer).Returns(new StringValues(redirectUrl));

            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                    })),
                }
            };

            var actual = await controller.NhsVacancy(request) as ViewResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual!.Model as NhsVacancyDetailsViewModel;

            actualModel!.Title.Should().Be(result.Vacancy!.Title);
            actualModel.EmployerName.Should().Be(result.Vacancy.EmployerName);
            actualModel.ApplicationUrl.Should().Be(result.Vacancy!.ApplicationUrl);
            actualModel.BackLinkUrl.Should().Be(redirectUrl);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Query_Returns_Null_Then_Not_Found_Returned(
            string vacancyReference,
            GetNhsApprenticeshipVacancyQueryResult result,
            GetVacancyDetailsRequest request,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IOptions<Domain.Configuration.FindAnApprenticeship>> faaConfig,
            [Frozen] Mock<IValidator<GetVacancyDetailsRequest>> validator,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Greedy] VacanciesController controller)
        {
            result.Vacancy = null;

            mediator.Setup(x => x.Send(It.Is<GetNhsApprenticeshipVacancyQuery>(c => c.VacancyReference == vacancyReference), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var actual = await controller.NhsVacancy(request) as NotFoundResult;

            actual!.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
