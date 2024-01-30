using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.GovUK.Auth.Models;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Service;
public class WhenStubSigningIn
{
    [Test, MoqAutoData]
    public void Then_The_Stub_Auth_Details_Are_Not_Returned_When_Prod(
        string returnUrl,
        StubAuthUserDetails model,
        [Frozen] Mock<IConfiguration> configuration,
        [Frozen] Mock<IStubAuthenticationService> stubAuthService,
        [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("prd");

        var actual = controller.StubSignedIn(returnUrl) as NotFoundResult;

        actual.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public void Then_The_Stub_Auth_Details_Are_Returned_When_Not_Prod(
        string emailClaimValue,
        string nameClaimValue,
        string returnUrl,
        StubAuthUserDetails model,
        [Frozen] Mock<IConfiguration> configuration,
        [Frozen] Mock<IStubAuthenticationService> stubAuthService,
        [Greedy] ServiceController controller)
    {
        configuration.Setup(x => x["ResourceEnvironmentName"]).Returns("test");
        var httpContext = new DefaultHttpContext();
        var emailClaim = new Claim(ClaimTypes.Email, emailClaimValue);
        var nameClaim = new Claim(ClaimTypes.NameIdentifier, nameClaimValue);
        var claimsPrinciple = new ClaimsPrincipal(new[] {new ClaimsIdentity(new[]
        {
            emailClaim,
            nameClaim
        })});
        httpContext.User = claimsPrinciple;
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var actual = controller.StubSignedIn(returnUrl) as ViewResult;
        var actualModel = actual?.Model as AccountStubViewModel;

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actualModel.Should().NotBeNull();
            actualModel.Email.Should().BeEquivalentTo(emailClaimValue);
            actualModel.Id.Should().BeEquivalentTo(nameClaimValue);
            actualModel.ReturnUrl.Should().BeEquivalentTo(returnUrl);
        }
    }
}
