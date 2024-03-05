using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class InterviewAdjustmentController(IMediator mediator) : Controller
    {
        public IActionResult Summary()
        {
            return View();
        }
    }
}
