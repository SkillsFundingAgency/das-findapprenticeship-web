using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.AppStart;

namespace SFA.DAS.FAA.Web.UnitTests;

public class TestControllerContextBuilder(ControllerContext controllerContext)
{
    public TestUserClaimBuilder WithUser(Guid userId)
    {
        controllerContext.HttpContext.User = new ClaimsPrincipal();
        var builder = new TestUserClaimBuilder(controllerContext.HttpContext.User);
        builder.WithClaim(CustomClaims.CandidateId, userId.ToString());
        return builder;
    }
}

public class TestUserClaimBuilder(ClaimsPrincipal claimsPrincipal)
{
    public TestUserClaimBuilder WithClaim(string type, string value)
    {
        if (claimsPrincipal.Identity is null)
        {
            claimsPrincipal.AddIdentity(
                new ClaimsIdentity(new List<Claim>
                {
                    new (type, value)
                }));
        }
        else
        {
            (claimsPrincipal.Identity as ClaimsIdentity)?.AddClaim(new Claim(type, value));
        }

        return this;
    }
}

public static class ControllerExtensions
{
    public static Controller WithUrlHelper(this Controller controller, Action<Mock<IUrlHelper>>? action = null)
    {
        ArgumentNullException.ThrowIfNull(controller);
        var urlHelper = new Mock<IUrlHelper>();
        controller.Url = urlHelper.Object; 
        action?.Invoke(urlHelper);
        return controller;
    }
    
    public static Controller WithContext(this Controller controller, Action<ControllerContext>? action = null)
    {
        ArgumentNullException.ThrowIfNull(controller);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        action?.Invoke(controller.ControllerContext);
        return controller;
    }
}

public static class ControllerContextExtensions
{
    public static TestUserClaimBuilder WithUser(this ControllerContext controllerContext, Guid userId)
    {
        controllerContext.HttpContext.User = new ClaimsPrincipal();
        var builder = new TestUserClaimBuilder(controllerContext.HttpContext.User);
        builder.WithClaim(CustomClaims.CandidateId, userId.ToString());
        return builder;
    }
}