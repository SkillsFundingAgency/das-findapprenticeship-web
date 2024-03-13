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
using static System.String;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class InterviewAdjustmentsController(IMediator mediator) : Controller
    {
        private const string ListViewPath = "~/Views/apply/interviewadjustments/List.cshtml";
        private const string SummaryViewPath = "~/Views/apply/InterviewAdjustments/Summary.cshtml";

        [Route("apply/{applicationId}/interview-adjustments",
            Name = RouteNames.ApplyApprenticeship.InterviewAdjustments)]
        public async Task<IActionResult> Index([FromRoute] Guid applicationId, [FromQuery] bool isEdit = false)
        {
            var result = await mediator.Send(new GetInterviewAdjustmentsQuery
            {
                ApplicationId = applicationId,
                CandidateId = User.Claims.CandidateId()
            });

            if (isEdit == false && result.Status is not null)
            {
                return RedirectToRoute(RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary, new { applicationId });
            }
            
            var model = new InterviewAdjustmentsViewModel
            {
                ApplicationId = applicationId,
                DoYouWantInterviewAdjustments = 
                    IsNullOrEmpty(result.InterviewAdjustmentsDescription) 
                        ? isEdit ? false : null 
                        : true,
                InterviewAdjustmentsDescription = result.InterviewAdjustmentsDescription,
            };

            return View(ListViewPath, model);
        }

        [HttpPost]
        [Route("apply/{applicationId}/interview-adjustments",
            Name = RouteNames.ApplyApprenticeship.InterviewAdjustments)]
        public async Task<IActionResult> Post([FromRoute] Guid applicationId, InterviewAdjustmentsViewModel viewModel, [FromQuery] bool isEdit = false)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetInterviewAdjustmentsQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                var model = new InterviewAdjustmentsViewModel
                {
                    ApplicationId = applicationId,
                    DoYouWantInterviewAdjustments =
                        IsNullOrEmpty(result.InterviewAdjustmentsDescription)
                            ? isEdit ? false : null
                            : true,
                    InterviewAdjustmentsDescription = result.InterviewAdjustmentsDescription
                };

                return View(ListViewPath, model);
            }

            var updateCommand = new UpdateInterviewAdjustmentsCommand
            {
                CandidateId = User.Claims.CandidateId(),
                ApplicationId = viewModel.ApplicationId,
                InterviewAdjustmentsDescription = viewModel.DoYouWantInterviewAdjustments!.Value
                    ? viewModel.InterviewAdjustmentsDescription
                    : string.Empty,
                InterviewAdjustmentsSectionStatus = SectionStatus.InProgress
            };

            await mediator.Send(updateCommand);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary,
                new {viewModel.ApplicationId});
        }
        
        [HttpGet]
        [Route("apply/{applicationId}/interview-adjustments/summary",
            Name = RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary)]
        public async Task<IActionResult> GetSummary([FromRoute] Guid applicationId)
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

        [HttpPost]
        [Route("apply/{applicationId}/interview-adjustments/summary",
            Name = RouteNames.ApplyApprenticeship.InterviewAdjustmentsSummary)]
        public async Task<IActionResult> PostSummary([FromRoute] Guid applicationId, InterviewAdjustmentSummaryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var result = await mediator.Send(new GetInterviewAdjustmentsQuery
                {
                    ApplicationId = applicationId,
                    CandidateId = User.Claims.CandidateId()
                });

                viewModel = new InterviewAdjustmentSummaryViewModel
                {
                    ApplicationId = applicationId,
                    SupportRequestAnswer = result.InterviewAdjustmentsDescription,
                    IsSupportRequestRequired = !IsNullOrEmpty(result.InterviewAdjustmentsDescription)
                };

                return View(SummaryViewPath, viewModel);
            }

            var updateCommand = new UpdateInterviewAdjustmentsCommand
            {
                CandidateId = User.Claims.CandidateId(),
                ApplicationId = viewModel.ApplicationId,
                InterviewAdjustmentsDescription = !IsNullOrEmpty(viewModel.SupportRequestAnswer) ? viewModel.SupportRequestAnswer : string.Empty,
                InterviewAdjustmentsSectionStatus = viewModel.IsSectionCompleted!.Value ? SectionStatus.Completed : SectionStatus.InProgress
            };

            await mediator.Send(updateCommand);

            return RedirectToRoute(RouteNames.Apply, new { applicationId });
        }
    }
}
