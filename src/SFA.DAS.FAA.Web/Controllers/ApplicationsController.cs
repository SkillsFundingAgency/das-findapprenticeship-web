using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
