using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("cookies")]
    public class CookiesController : Controller
    {
        [AllowAnonymous]
        [Route("", Name = RouteNames.Cookies)]
        [HttpGet]
        public IActionResult Index()
        {
            var analyticsCookieValue = Request.Cookies[CookieKeys.AnalyticsCookieConsent];
            var functionalCookieValue = Request.Cookies[CookieKeys.FunctionalCookieConsent];

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
        [HttpPost]
        [Route("", Name = RouteNames.Cookies)]
        public IActionResult Index(CookiesViewModel viewModel)
        {
            DeleteCookie(CookieKeys.AnalyticsCookieConsent);
            DeleteCookie(CookieKeys.FunctionalCookieConsent);

            SetCookie(CookieKeys.AnalyticsCookieConsent, viewModel.ConsentAnalyticsCookie.ToString());
            SetCookie(CookieKeys.FunctionalCookieConsent, viewModel.ConsentFunctionalCookie.ToString());
            
            viewModel.ShowBannerMessage = true;

            return View(viewModel);
        }

        private void SetCookie(string cookieName, string value)
        {
            Response.Cookies.Append(cookieName, value, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1),
                Path = "/"
            });
        }

        private void DeleteCookie(string cookieName)
        {
            if (Request.Cookies[cookieName] != null)
            {
                Response.Cookies.Append(cookieName, "", new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1)
                });
            }
        }
    }
}