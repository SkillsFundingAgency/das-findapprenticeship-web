using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";

        [HttpGet]
        [Route("apprenticeship/apply/workhistory", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public IActionResult Get()
        {
            return View(ViewPath);
        }

        [HttpGet]
        [Route("apprenticeship/apply/workhistory", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public IActionResult Post(Applicationse)
        {
            return View(ViewPath);
        }
    }
}