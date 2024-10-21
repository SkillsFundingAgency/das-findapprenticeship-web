using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Home
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        [MoqInlineAutoData(true, null, "https://baseUrl/apprenticeshipsearch")]
        [MoqInlineAutoData(false, null, "https://baseUrl/apprenticeshipsearch")]
        [MoqInlineAutoData(true, "some url", "some url")]
        [MoqInlineAutoData(false, "some url", "some url")]
        public void Then_Cookies_View_Is_Returned(
            bool cookieValue,
            string previousPageUrl,
            string expectedUrl)
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(expectedUrl);

            var cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.SetupGet(c => c[CookieKeys.AnalyticsConsent]).Returns(cookieValue.ToString);
            cookiesMock.SetupGet(c => c[CookieKeys.FunctionalConsent]).Returns(cookieValue.ToString);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(ctx => ctx.Request.Cookies).Returns(cookiesMock.Object);
            httpContextMock.Setup(ctx => ctx.Request.Headers.Referer).Returns(new StringValues(previousPageUrl));

            var controller = new HomeController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                },
                Url = mockUrlHelper.Object
            };
            var result = controller.Cookies() as ViewResult;

            result.Should().NotBeNull();

            var actualModel = result!.Model as CookiesViewModel;
            
            actualModel.Should().NotBeNull();
            actualModel!.ShowBannerMessage.Should().BeFalse();
            actualModel.PreviousPageUrl.Should().Be(expectedUrl);
            actualModel.ConsentAnalyticsCookie.Should().Be(cookieValue);
            actualModel.ConsentFunctionalCookie.Should().Be(cookieValue);

        }

        [Test, MoqAutoData]
        public void Then_AccessibilityStatement_View_Is_Returned([Greedy] HomeController controller)
        {
            var result = controller.AccessibilityStatement() as ViewResult;

            result.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public void Then_Terms_AndConditions_View_Is_Returned([Greedy] HomeController controller)
        {
            var result = controller.TermsAndConditions() as ViewResult;

            result.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public void Then_Get_Help_View_Is_Returned([Greedy] HomeController controller)
        {
            var result = controller.GetHelp() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}