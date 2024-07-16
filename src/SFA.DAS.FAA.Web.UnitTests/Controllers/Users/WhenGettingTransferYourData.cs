using System.Security.Claims;
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
using SFA.DAS.FAA.Web.AppStart;
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
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned_And_Back_Link_Set_To_Cached_Value(
            string expectedUrl,
            string govIdentifier,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-{CacheKeys.CreateAccountReturnUrl}"))
                .ReturnsAsync(expectedUrl);
            var controller = new UserController(mediator.Object, cacheStorageService.Object, Mock.Of<IConfiguration>(), Mock.Of<IOidcService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        }))
                    }
                }
            };

            var result = await controller.TransferYourData() as ViewResult;

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            var actualModel = result!.Model as TransferYourDataViewModel;

            actualModel.Should().NotBeNull();
            actualModel!.PreviousPageUrl.Should().NotBeNull();
            actualModel.PreviousPageUrl.Should().Be(expectedUrl);
        }
        
        [Test, MoqAutoData]
        public async Task Then_View_Is_Returned_And_Back_Link_Set_To_Applications_If_No_Cached_Value(
            string expectedUrl,
            string govIdentifier,
            [Frozen] Mock<IUrlHelper> urlHelper,
            [Frozen] Mock<IMediator> mediator,
            [Frozen] Mock<ICacheStorageService> cacheStorageService)
        {
            cacheStorageService.Setup(x => x.Get<string>($"{govIdentifier}-{CacheKeys.CreateAccountReturnUrl}"))
                .ReturnsAsync((string)null);
            urlHelper.Setup(x => x.RouteUrl(It.Is<UrlRouteContext>(c=>c.RouteName == RouteNames.Applications.ViewApplications))).Returns(expectedUrl);
            var controller = new UserController(mediator.Object, cacheStorageService.Object, Mock.Of<IConfiguration>(), Mock.Of<IOidcService>())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                        }))
                    }
                },
                Url = urlHelper.Object
            };

            var result = await controller.TransferYourData() as ViewResult;

            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            var actualModel = result!.Model as TransferYourDataViewModel;

            actualModel.Should().NotBeNull();
            actualModel!.PreviousPageUrl.Should().NotBeNull();
            actualModel.PreviousPageUrl.Should().Be(expectedUrl);
        }
    }
}