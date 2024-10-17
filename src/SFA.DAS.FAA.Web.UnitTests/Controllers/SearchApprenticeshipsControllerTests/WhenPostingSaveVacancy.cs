using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Commands.Vacancy.SaveVacancy;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAT.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using System.Security.Policy;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.SearchApprenticeshipsControllerTests
{
    [TestFixture]
    public class WhenPostingSaveVacancy
    {
        [Test]
        [MoqInlineAutoData("https://baseUrl/apprenticeshipsearch")]
        [MoqInlineAutoData("https://baseUrl/apprenticeshipsearch?query1=somevalue&query2=someValue2")]
        public async Task Then_If_Command_Returns_Redirect_Returned(
            string redirectUrl,
            Guid candidateId,
            string vacancyReference,
            SaveVacancyCommandResult mediatorResult,
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
                    })),
                }
            };
            mediator.Setup(x => x.Send(It.IsAny<SaveVacancyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.SearchResultsSaveVacancy(vacancyReference) as RedirectResult;

            actual!.Url.Should().Be(redirectUrl);
        }

        [Test, MoqAutoData]
        public async Task Then_Page_Referer_Is_Null_Command_Returns_Redirect_Returned(
            Guid candidateId,
            string vacancyReference,
            SaveVacancyCommandResult mediatorResult,
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
            mediator.Setup(x => x.Send(It.IsAny<SaveVacancyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.SearchResultsSaveVacancy(vacancyReference) as RedirectResult;

            actual!.Url.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Query_With_Redirect_Command_Returns_JsonOk_Returned(
            Guid candidateId,
            string vacancyReference,
            SaveVacancyCommandResult mediatorResult,
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
                    }))

                }
            };
            mediator.Setup(x => x.Send(It.IsAny<SaveVacancyCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.SearchResultsSaveVacancy(vacancyReference, false) as JsonResult;

            actual!.Value.Should().Be(StatusCodes.Status200OK);
        }
    }
}
