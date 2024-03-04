using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;

namespace SFA.DAS.FAA.Web.Controllers.Apply;

[Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
public class InterviewAdjustmentsController(IMediator mediator) : Controller
{
    private const string ViewPath = "~/Views/apply/interviewadjustments/List.cshtml";

    [HttpGet]
    [Route("apply/{applicationId}/interview-adjustments/", Name = RouteNames.ApplyApprenticeship.InterviewAdjustments)]
    public async Task<IActionResult> Index([FromRoute] Guid applicationId)
    {
        var result = await mediator.Send(new GetInterviewAdjustmentsQuery
        {
            ApplicationId = applicationId,
            CandidateId = User.Claims.CandidateId()
        });

        var model = new InterviewAdjustmentsViewModel()
        {
            ApplicationId = applicationId,
            DoYouWantInterviewAdjustments = result.Status,
            InterviewAdjustmentsDescription = result.InterviewAdjustmentsDescription
        };

        return View(ViewPath, model);
    }
}
