using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.InterviewAdjustments;
using SFA.DAS.FAA.Application.Queries.Apply.GetInterviewAdjustments;
using SFA.DAS.FAA.Domain.Enums;
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

    [HttpPost]
    [Route("apply/{applicationId}/interview-adjustments/", Name = RouteNames.ApplyApprenticeship.InterviewAdjustments)]
    public async Task<IActionResult> Post([FromRoute] Guid applicationId, InterviewAdjustmentsViewModel viewModel)
    {
        if (!ModelState.IsValid)
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

        var updateCommand = new UpdateInterviewAdjustmentsCommand
        {
            CandidateId = User.Claims.CandidateId(),
            ApplicationId = viewModel.ApplicationId,
            InterviewAdjustmentsDescription = viewModel.InterviewAdjustmentsDescription,
            InterviewAdjustmentsSectionStatus = viewModel.DoYouWantInterviewAdjustments!.Value ? SectionStatus.InProgress : SectionStatus.Completed
        };

        await mediator.Send(updateCommand);

        return viewModel.DoYouWantInterviewAdjustments.Value
            ? RedirectToRoute(RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary, new { viewModel.ApplicationId })
            : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
    }
}
