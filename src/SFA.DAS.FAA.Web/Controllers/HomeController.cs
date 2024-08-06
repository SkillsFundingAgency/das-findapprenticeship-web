using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Attributes;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("[controller]")]
    [AllowIncompleteAccountAccess]
    public class HomeController : Controller
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

        [AllowAnonymous]
        [Route("accessibility-statement", Name = RouteNames.AccessibilityStatement)]
        [HttpGet]
        public IActionResult AccessibilityStatement()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("terms-and-conditions", Name = RouteNames.TermsAndConditions)]
        [HttpGet]
        public IActionResult TermsAndConditions()
        {
            return View();
        }
    }
}