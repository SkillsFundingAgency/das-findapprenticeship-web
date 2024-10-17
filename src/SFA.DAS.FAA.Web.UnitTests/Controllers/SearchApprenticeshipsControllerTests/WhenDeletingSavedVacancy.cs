using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Vacancy.DeleteSavedVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using JsonResult = Microsoft.AspNetCore.Mvc.JsonResult;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests
{
    [TestFixture]
    public class WhenDeletingSavedVacancy
    {
        [Test]
        [MoqInlineAutoData("https://baseUrl/apprenticeshipsearch")]
        [MoqInlineAutoData("https://baseUrl/apprenticeshipsearch?query1=somevalue&query2=someValue2")]
        public async Task Then_If_Command_Returns_Redirect_Returned(
            string redirectUrl,
            Guid candidateId,
            string vacancyReference,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
        {
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
                    }))
                }
            };

            var actual = await controller.SearchResultsDeleteSavedVacancy(vacancyReference, true) as RedirectResult;

            actual!.Url.Should().Be(redirectUrl);

            mediator.Verify(x => x.Send(It.IsAny<DeleteSavedVacancyCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Page_Referer_Is_Null_Command_Returns_Redirect_Returned(
            Guid candidateId,
            string vacancyReference,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.SearchApprenticeshipsController controller)
        {
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

            var actual = await controller.SearchResultsDeleteSavedVacancy(vacancyReference, true) as RedirectResult;

            actual!.Url.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Command_Returns_JsonOk_Returned(
            Guid candidateId,
            string vacancyReference,
            IDateTimeService dateTimeService,
            [Frozen] Mock<IUrlHelper> mockUrlHelper,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Web.Controllers.VacanciesController controller)
        {
            controller.Url = mockUrlHelper.Object;
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(CustomClaims.CandidateId, candidateId.ToString()),
                    }))
                }
            };

            var actual = await controller.VacancyDetailsDeleteSavedVacancy(vacancyReference, false) as JsonResult;

            actual!.Value.Should().Be(StatusCodes.Status200OK);

            mediator.Verify(x => x.Send(It.IsAny<DeleteSavedVacancyCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}