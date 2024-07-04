using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        [Route("Cookies", Name = RouteNames.Cookies)]
        [HttpGet]
        public IActionResult Cookies()
        {
            var analyticsCookieValue = Request.Cookies[CookieKeys.AnalyticsConsent];
            var functionalCookieValue = Request.Cookies[CookieKeys.FunctionalConsent];

            bool.TryParse(analyticsCookieValue, out var isAnalyticsCookieConsentGiven);
            bool.TryParse(functionalCookieValue, out var isFunctionalCookieConsentGiven);

            var cookieViewModel = new CookiesViewModel
            {
                PreviousPageUrl = Request.Headers["Referer"].ToString(),
                ShowBannerMessage = false,
                ConsentAnalyticsCookie = isAnalyticsCookieConsentGiven,
                ConsentFunctionalCookie = isFunctionalCookieConsentGiven
            };
            return View(cookieViewModel);
        }

        [AllowAnonymous]
        [Route("accessibility-statement", Name = RouteNames.AccessibilityStatement)]
        [HttpGet]
        public IActionResult AccessibilityStatement()
        {
            return View();
        }
    }
}