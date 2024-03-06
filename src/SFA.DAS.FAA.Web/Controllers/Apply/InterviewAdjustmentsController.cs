using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Queries.Apply.InterviewAdjustments;
using SFA.DAS.FAA.Web.Authentication;
using SFA.DAS.FAA.Web.Extensions;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using static System.String;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class InterviewAdjustmentsController(IMediator mediator) : Controller
    {
        private const string SummaryViewPath = "~/Views/apply/InterviewAdjustments/Summary.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/interview-adjustments/summary", Name = RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary)]
        public async Task<IActionResult> Summary([FromRoute] Guid applicationId)
        {
            var result = await mediator.Send(new GetInterviewAdjustmentsQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            var viewModel = new InterviewAdjustmentSummaryViewModel
            {
                ApplicationId = applicationId,
                SupportRequestAnswer = result.InterviewAdjustmentsDescription,
                IsSupportRequestRequired = !IsNullOrEmpty(result.InterviewAdjustmentsDescription)
            };

            return View(SummaryViewPath, viewModel);
        }
    }
}
