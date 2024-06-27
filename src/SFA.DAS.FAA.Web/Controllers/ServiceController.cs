using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.GovUK.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Models;
using System.Security.Claims;

namespace SFA.DAS.FAA.Web.Controllers;

[Route("[controller]")]
public class ServiceController(IStubAuthenticationService stubAuthenticationService, IConfiguration configuration, IDataProtectorService dataProtectorService) : Controller
{
    [Route("signout", Name = RouteNames.SignOut)]
    public async Task<IActionResult> SignOut()
    {
        var idToken = await HttpContext.GetTokenAsync("id_token");

        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.Parameters.Clear();
        authenticationProperties.Parameters.Add("id_token", idToken);

        var schemes = new List<string>
        {
            CookieAuthenticationDefaults.AuthenticationScheme
        };
        _ = bool.TryParse(configuration["StubAuth"], out var stubAuth);
        if (!stubAuth)
        {
            schemes.Add(OpenIdConnectDefaults.AuthenticationScheme);
        }

        return SignOut(
            authenticationProperties,
            schemes.ToArray());
    }

    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("signin", Name = RouteNames.SignIn)]
    public async Task<IActionResult> SignIn([FromQuery] string signInValue)
    {
        var protectedValue = dataProtectorService.DecodeData(signInValue);

        if (string.IsNullOrEmpty(protectedValue))
        {
            return RedirectToRoute(RouteNames.ServiceStartDefault);
        }

        var values = protectedValue.Split('|');
        var controllerName = values[0];
        var actionName = values[1];
        var vacancyReference = values[2];
        return string.IsNullOrEmpty(vacancyReference) 
            ? RedirectToAction(actionName, controllerName) 
            : RedirectToAction(actionName, controllerName, new {vacancyReference});
    }

    [AllowAnonymous]
    [Route("user-signed-out", Name = RouteNames.SignedOut)]
    [HttpGet]
    public IActionResult SignedOut()
    {
        return View("SignedOut", new SignedOutViewModel(configuration["ResourceEnvironmentName"]));
    }

    [Authorize(Policy = nameof(PolicyNames.IsAuthenticated))]
    [Route("account-unavailable", Name = RouteNames.AccountUnavailable)]
    public IActionResult AccountUnavailable()
    {
        return View();
    }

    [HttpGet]
    [Route("account-details", Name = RouteNames.StubAccountDetailsGet)]
    public IActionResult AccountDetails([FromQuery] string returnUrl)
    {
        if (configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
        {
            return NotFound();
        }

        return View("AccountDetails", new StubAuthenticationViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [Route("account-details", Name = RouteNames.StubAccountDetailsPost)]
    public async Task<IActionResult> AccountDetails(StubAuthenticationViewModel model)
    {
        if (configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
        {
            return NotFound();
        }

        var claims = await stubAuthenticationService.GetStubSignInClaims(model);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claims,
            new AuthenticationProperties());

        return RedirectToRoute(RouteNames.StubSignedIn, new { returnUrl = model.ReturnUrl });
    }

    [HttpGet]
    [Authorize(Policy = nameof(PolicyNames.IsAuthenticated))]
    [Route("Stub-Auth", Name = RouteNames.StubSignedIn)]
    public IActionResult StubSignedIn([FromQuery] string returnUrl)
    {
        if (configuration["ResourceEnvironmentName"].ToUpper() == "PRD")
        {
            return NotFound();
        }
        var viewModel = new AccountStubViewModel
        {
            Email = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value,
            Id = User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))?.Value,
            ReturnUrl = returnUrl
        };
        return View(viewModel);
    }
}
