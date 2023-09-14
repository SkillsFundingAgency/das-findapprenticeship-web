using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers;

public class HomeController : Controller
{
    [Route("", Name = RouteNames.ServiceStartDefault, Order = 0)]
    public IActionResult Index()
    {
        return View();
    }
}

