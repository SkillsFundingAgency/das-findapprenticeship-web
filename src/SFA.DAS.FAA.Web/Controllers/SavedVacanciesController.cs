using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    [Route("saved-vacancies")]
    public class SavedVacanciesController : Controller
    {
        [Route("", Name = RouteNames.SavedVacancies)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
