using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers
{
    public class UserController : Controller
    {
        [Route("user-name", Name = RouteNames.UserName)]
        public IActionResult Name()
        {
            return View();
        }
    }
}
