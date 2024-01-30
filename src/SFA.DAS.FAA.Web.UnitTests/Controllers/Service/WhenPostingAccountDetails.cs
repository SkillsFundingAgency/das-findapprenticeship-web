using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.Testing.AutoFixture;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Service;
public class WhenPostingAccountDetails
{
    [Test, MoqAutoData]
    public async Task Then_The_Stub_Auth_Is_Created_When_Not_Prod(
    ClaimsPrincipal claimsPrincipal,
    StubAuthenticationViewModel model,
    [Frozen] Mock<IUrlHelperFactory> urlHelperFactory,
    [Frozen] Mock<IAuthenticationService> authService,
    [Frozen] Mock<IConfiguration> configuration,
    [Frozen] Mock<IStubAuthenticationService> stubAuthService,
    [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");
        stubAuthService.Setup(x => x.GetStubSignInClaims(model)).ReturnsAsync(claimsPrincipal);

        var httpContext = new DefaultHttpContext();

        var httpContextRequestServices = new Mock<IServiceProvider>();
        httpContextRequestServices.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(authService.Object);
        httpContextRequestServices.Setup(x => x.GetService(typeof(IUrlHelperFactory))).Returns(urlHelperFactory.Object);
        httpContext.RequestServices = httpContextRequestServices.Object;

        var controllerContext = new ControllerContext { HttpContext = httpContext };
        controller.ControllerContext = controllerContext;

        var actual = await controller.AccountDetails(model) as RedirectToRouteResult;

        using (new AssertionScope())
        {
            actual.RouteName.Should().Be(RouteNames.StubSignedIn);
            stubAuthService.Verify(x => x.GetStubSignInClaims(model), Times.Once);
            authService.Verify(x => x.SignInAsync(httpContext, CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, It.IsAny<AuthenticationProperties?>()), Times.Once);

        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Stub_Auth_Is_Not_Created_When_Prod(
        StubAuthenticationViewModel model,
        [Frozen] Mock<IAuthenticationService> authService,
        [Frozen] Mock<IConfiguration> configuration,
        [Frozen] Mock<IStubAuthenticationService> stubAuthService,
        [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("prd");
        var httpContext = new DefaultHttpContext();

        var httpContextRequestServices = new Mock<IServiceProvider>();
        httpContextRequestServices.Setup(x => x.GetService(typeof(IAuthenticationService))).Returns(authService.Object);
        var controllerContext = new ControllerContext { HttpContext = httpContext };
        controller.ControllerContext = controllerContext;

        var actual = await controller.AccountDetails(model) as NotFoundResult;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            stubAuthService.Verify(x => x.GetStubSignInClaims(It.IsAny<StubAuthenticationViewModel>()), Times.Never);
            authService.Verify(x => x.SignInAsync(httpContext, CookieAuthenticationDefaults.AuthenticationScheme, It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties?>()), Times.Never);
        }
    }
}
