using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";

        [HttpGet]
        [Route("vacancies/{vacancyReference}/apply/workhistory", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public IActionResult Get()
        {
            return View(ViewPath, new WorkHistoryListViewModel());
        }

        [HttpPost]
        [Route("vacancies/{vacancyReference}/apply/workhistory", Name = RouteNames.ApplyApprenticeship.WorkHistory)]
        public IActionResult Post(WorkHistoryListViewModel model)
        {
            return View(ViewPath);
        }
    }
}