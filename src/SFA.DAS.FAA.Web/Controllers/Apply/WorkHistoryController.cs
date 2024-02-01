using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Application.Queries.Apply.GetWorkHistories;
using SFA.DAS.FAA.Domain.Enums;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.Authentication;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";
        private const string SummaryViewPath = "~/Views/apply/workhistory/Summary.cshtml";

        [HttpGet]
        [Route("apply/{applicationId}/jobs/", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public IActionResult Get([FromRoute] Guid applicationId)
        {
            return View(ViewPath, new AddWorkHistoryViewModel
            {
                ApplicationId = applicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId })
            });
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/", Name = RouteNames.ApplyApprenticeship.Jobs)]
        public async Task<IActionResult> Post(AddWorkHistoryViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.AddJob))
            {
                ModelState.AddModelError(nameof(viewModel.AddJob), "Select if you want to add any jobs");
                viewModel.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId });
                return View(ViewPath, viewModel);
            }

            var command = new UpdateApplicationCommand
            {
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                ApplicationId = viewModel.ApplicationId,
                WorkHistorySectionStatus = viewModel.AddJob is "Yes" ? SectionStatus.InProgress : SectionStatus.Completed
            };

            await mediator.Send(command);

            return viewModel.AddJob.Equals("Yes")
                ? RedirectToRoute(RouteNames.ApplyApprenticeship.AddJob, new { viewModel.ApplicationId })
                : RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
        }

        [HttpGet]
        [Route("apply/{applicationId}/jobs/add", Name = RouteNames.ApplyApprenticeship.AddJob)]
        public IActionResult GetAddAJob([FromRoute] Guid applicationId)
        {
            var viewModel = new AddJobViewModel
            {
                ApplicationId = applicationId
            };

            return View("~/Views/apply/workhistory/AddJob.cshtml", viewModel);
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/add", Name = RouteNames.ApplyApprenticeship.AddJob)]
        public async Task<IActionResult> PostAddAJob(AddJobViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/apply/workhistory/AddJob.cshtml", request);
            }

            var command = new AddJobCommand
            {
                ApplicationId = request.ApplicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                EmployerName = request.EmployerName,
                JobDescription = request.JobDescription,
                JobTitle = request.JobTitle,
                StartDate = request.StartDate.DateTimeValue.Value,
                EndDate = request.IsCurrentRole is true ? null : request.EndDate?.DateTimeValue
            };

            await mediator.Send(command);

            return RedirectToRoute(RouteNames.ApplyApprenticeship.Jobs, new { request.ApplicationId });
        }
		
		[HttpGet]
        [Route("apply/{applicationId}/jobs/summary", Name = RouteNames.ApplyApprenticeship.JobsSummary)]
        public async Task<IActionResult> Summary([FromRoute] Guid applicationId)
        {
            var query = new GetJobsQuery
            {
                ApplicationId = applicationId,
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
            };

            var result = await mediator.Send(query);

            return View(SummaryViewPath, new WorkHistorySummaryViewModel
            {
                WorkHistories = result.Jobs.Select(wk => (WorkHistoryViewModel)wk).ToList(),
                ApplicationId = applicationId,
                BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { applicationId }),
                ChangeLinkUrl = string.Empty, //TODO: Redirect the user to the Edit Page
                DeleteLinkUrl = string.Empty, //TODO: Redirect the user to the Delete Page
                AddAnotherJobLinkUrl = Url.RouteUrl(RouteNames.ApplyApprenticeship.AddJob, new { applicationId }),
            });
        }

        [HttpPost]
        [Route("apply/{applicationId}/jobs/summary", Name = RouteNames.ApplyApprenticeship.JobsSummary)]
        public async Task<IActionResult> Summary(WorkHistorySummaryViewModel viewModel)
        {
            if (viewModel.IsSectionCompleted is null)
            {
                ModelState.AddModelError(nameof(viewModel.IsSectionCompleted), "Select if you have finished this section");
                var result = await mediator.Send(new GetJobsQuery
                {
                    ApplicationId = viewModel.ApplicationId,
                    CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value)
                });
                viewModel.BackLinkUrl = Url.RouteUrl(RouteNames.Apply, new { viewModel.ApplicationId });
                viewModel.ChangeLinkUrl = string.Empty; //TODO: Redirect the user to the Edit Page
                viewModel.DeleteLinkUrl = string.Empty; //TODO: Redirect the user to the Delete Page
                viewModel.AddAnotherJobLinkUrl = Url.RouteUrl(RouteNames.ApplyApprenticeship.AddJob, new { viewModel.ApplicationId });
                viewModel.WorkHistories = result.Jobs.Select(wk => (WorkHistoryViewModel)wk).ToList();
                return View(SummaryViewPath, viewModel);
            }

            var command = new UpdateApplicationCommand
            {
                CandidateId = Guid.Parse(User.Claims.First(c => c.Type.Equals(CustomClaims.CandidateId)).Value),
                ApplicationId = viewModel.ApplicationId,
                WorkHistorySectionStatus = (SectionStatus)viewModel.IsSectionCompleted
            };

            await mediator.Send(command);

            return RedirectToRoute(RouteNames.Apply, new { viewModel.ApplicationId });
        }
    }
}