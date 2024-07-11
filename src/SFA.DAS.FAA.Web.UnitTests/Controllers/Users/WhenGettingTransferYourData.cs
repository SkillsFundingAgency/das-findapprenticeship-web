using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users
{
    [TestFixture]
    public class WhenGettingTransferYourData
    {
        [Test]
        [MoqInlineAutoData(null, "https://baseUrl/create-account")]
        [MoqInlineAutoData("some url", "some url")]
        public void Then_View_Is_Returned(
            string previousPageUrl,
            string expectedUrl,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns(expectedUrl);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(ctx => ctx.Request.Headers.Referer).Returns(new StringValues(previousPageUrl));

            var controller = new UserController(mediator.Object, cacheStorageService.Object, Mock.Of<IConfiguration>(), Mock.Of<IOidcService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContextMock.Object
                },
                Url = mockUrlHelper.Object
            };

            var result = controller.TransferYourData() as ViewResult;

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            var actualModel = result!.Model as TransferYourDataViewModel;

            actualModel.Should().NotBeNull();
            actualModel!.PreviousPageUrl.Should().NotBeNull();
            actualModel.PreviousPageUrl.Should().Be(expectedUrl);
        }
    }
}