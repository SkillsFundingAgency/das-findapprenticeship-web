using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]

    public class DisabilityConfidentController : Controller
    {
        [HttpGet]
        [Route("apply/{applicationId}/disability-confident", Name = RouteNames.ApplyApprenticeship.DisabilityConfident)]
        public IActionResult Index()
        {
            return View("~/Views/Apply/DisabilityConfident/Index.cshtml");
        }
    }
}
