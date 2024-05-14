using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("404", Name = RouteNames.Error.Error404)]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("500", Name = RouteNames.Error.Error500)]
        public IActionResult ApplicationError()
        {
            return View();
        }
    }
}
