using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Controllers;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Controllers.Users;

public class WhenPostingConfirmYourAccountDetails
{
    private static void Arrange(UserController controller, Guid candidateId, string govIdentifier, bool isLocal)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, govIdentifier),
                    new Claim(CustomClaims.CandidateId, candidateId.ToString())
                }))
            }
        };
        var urlHelper = new Mock<IUrlHelper>();
        urlHelper.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(isLocal);
        controller.Url = urlHelper.Object;
    }

    [Test, MoqAutoData]
    public async Task Then_Local_ReturnUrl_Is_Redirected_To(
        Guid candidateId,
        string govIdentifier,
        ConfirmAccountDetailsViewModel model,
        [Frozen] Mock<ICacheStorageService> cache,
        [Greedy] UserController controller)
    {
        Arrange(controller, candidateId, govIdentifier, true);
        cache.Setup(x => x.Get<string>($"{govIdentifier}-{CacheKeys.CreateAccountReturnUrl}")).ReturnsAsync("/local/path");

        var result = await controller.ConfirmYourAccountDetails(model);

        result.Should().BeOfType<LocalRedirectResult>().Which.Url.Should().Be("/local/path");
    }

    [Test, MoqAutoData]
    public async Task Then_NonLocal_ReturnUrl_Redirects_To_ServiceStart(
        Guid candidateId,
        string govIdentifier,
        ConfirmAccountDetailsViewModel model,
        [Frozen] Mock<ICacheStorageService> cache,
        [Greedy] UserController controller)
    {
        Arrange(controller, candidateId, govIdentifier, false);
        cache.Setup(x => x.Get<string>($"{govIdentifier}-{CacheKeys.CreateAccountReturnUrl}")).ReturnsAsync("https://attacker.example");

        var result = await controller.ConfirmYourAccountDetails(model);

        result.Should().BeOfType<RedirectToRouteResult>().Which.RouteName.Should().Be(RouteNames.ServiceStartDefault);
    }
}
