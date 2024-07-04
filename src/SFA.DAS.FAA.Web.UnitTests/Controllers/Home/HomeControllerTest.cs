using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Home
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test, MoqAutoData]
        public void Then_Cookies_View_Is_Returned(
            bool cookieValue,
            string previousPageUrl)
        {
            var cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.SetupGet(c => c["AnalyticsCookieConsent"]).Returns(cookieValue.ToString);
            cookiesMock.SetupGet(c => c["FunctionalCookieConsent"]).Returns(cookieValue.ToString);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(ctx => ctx.Request.Cookies).Returns(cookiesMock.Object);
            httpContextMock.Setup(ctx => ctx.Request.Headers["Referer"]).Returns(previousPageUrl);

            var controller = new HomeController
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                },
            };
            var result = controller.Cookies() as ViewResult;

            result.Should().NotBeNull();

            var actualModel = result!.Model as CookiesViewModel;
            
            actualModel.Should().NotBeNull();
            actualModel!.ShowBannerMessage.Should().BeFalse();
            actualModel.PreviousPageUrl.Should().Be(previousPageUrl);
            actualModel.ConsentAnalyticsCookie.Should().Be(cookieValue);
            actualModel.ConsentFunctionalCookie.Should().Be(cookieValue);

        }

        [Test, MoqAutoData]
        public void Then_AccessibilityStatement_View_Is_Returned([Greedy] HomeController controller)
        {
            var result = controller.AccessibilityStatement() as ViewResult;

            result.Should().NotBeNull();
        }
    }
}