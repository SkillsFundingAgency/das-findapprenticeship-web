using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Infrastructure;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class InterviewAdjustmentController(IMediator mediator) : Controller
    {
        [HttpGet]
        [Route("apply/{applicationId}/interview-adjustment/summary", Name = RouteNames.ApplyApprenticeship.InterviewAdjustmentSummary)]
        public IActionResult Summary([FromRoute] Guid applicationId)
        {
            return View();
        }
    }
}
