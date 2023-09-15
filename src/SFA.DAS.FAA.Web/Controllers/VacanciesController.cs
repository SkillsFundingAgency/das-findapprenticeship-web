using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Controllers
{
    public class VacanciesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
