using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Application.Commands.UpdateApplication;
using SFA.DAS.FAA.Application.Commands.WorkHistory.AddJob;
using SFA.DAS.FAA.Domain.Apply.UpdateApplication.Enums;
using SFA.DAS.FAA.Web.Infrastructure;
using SFA.DAS.FAA.Web.Models.Apply;
using SFA.DAS.FAA.Web.AppStart;
using SFA.DAS.FAA.Web.Authentication;

namespace SFA.DAS.FAA.Web.Controllers.Apply
{
    [Authorize(Policy = nameof(PolicyNames.IsFaaUser))]
    public class WorkHistoryController(IMediator mediator) : Controller
    {
        private const string ViewPath = "~/Views/apply/workhistory/List.cshtml";

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
                CandidateId =  Guid.Parse(User.Claims.First(c=>c.Type.Equals(CustomClaims.CandidateId)).Value),
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
        [Route("apply/{applicationId}/jobs/delete", Name = RouteNames.ApplyApprenticeship.DeleteJob)]
        public IActionResult DeleteJob([FromRoute] Guid applicationId)
        {
                JobId = request.JobId,
            return View("~/Views/apply/workhistory/DeleteJob.cshtml");
        }
    }
}