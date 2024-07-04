using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("[controller]")]
    public class HomeController() : Controller
    {
        [AllowAnonymous]
        [Route("Cookies", Name = RouteNames.Cookies)]
        [HttpGet]
        public IActionResult Cookies()
        {
            var analyticsCookieValue = Request.Cookies[CookieKeys.AnalyticsConsent];
            var functionalCookieValue = Request.Cookies[CookieKeys.FunctionalConsent];

            _ = bool.TryParse(analyticsCookieValue, out var isAnalyticsCookieConsentGiven);
            _ = bool.TryParse(functionalCookieValue, out var isFunctionalCookieConsentGiven);

            var referer = Request.Headers.Referer.FirstOrDefault();

            var cookieViewModel = new CookiesViewModel
            {
                PreviousPageUrl = referer ?? Url.RouteUrl(RouteNames.ServiceStartDefault) ?? "/",
                ShowBannerMessage = false,
                ConsentAnalyticsCookie = isAnalyticsCookieConsentGiven,
                ConsentFunctionalCookie = isFunctionalCookieConsentGiven
            };
            return View(cookieViewModel);
        }
    }
}